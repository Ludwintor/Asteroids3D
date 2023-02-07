using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX;
using BoundingBox = Microsoft.Xna.Framework.BoundingBox;
using Color = Microsoft.Xna.Framework.Color;
using Matrix = Microsoft.Xna.Framework.Matrix;
using Quaternion = Microsoft.Xna.Framework.Quaternion;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Vector3 = Microsoft.Xna.Framework.Vector3;

namespace Asteroids3D
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private KeyboardState _lastKeyboard;
        private MouseState _lastMouse;

        private BasicEffect _fx;
        private GameObject _cone;
        private Octree _octree;
        private Random _rng;
        private Camera _camera;
        private List<BoxCollider> _colliders;

        private int _lastScrollValue;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this)
            {
                IsFullScreen = false,
                PreferredBackBufferWidth = 1600,
                PreferredBackBufferHeight = 900
            };
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
        }

        protected override void Initialize()
        {
            _colliders = new List<BoxCollider>();
            _camera = new Camera(this)
            {
                Position = new Vector3(0f, 0f, 5f)
            };
            _fx = new BasicEffect(GraphicsDevice)
            {
                VertexColorEnabled = true,
                Projection = _camera.Projection
            };
            _cone = new GameObject
            {
                MeshRenderer = new MeshRenderer
                {
                    Mesh = new ConeMesh(GraphicsDevice, 1f, 0.5f, Color.Green, Color.Red, 4),
                    Effect = _fx
                }
            };
            _octree = new Octree(new BoundingBox(Vector3.One * -5f, Vector3.One * 5f), _fx);
            _rng = new Random();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();
            MouseState ms = Mouse.GetState();
            if (ks.IsKeyDown(Keys.Escape))
                Exit();

            if (ks.IsKeyDown(Keys.Up))
                _cone.Rotation *= Quaternion.CreateFromRotationMatrix(Matrix.CreateRotationX(-0.05f));
            if (ks.IsKeyDown(Keys.Down))
                _cone.Rotation *= Quaternion.CreateFromRotationMatrix(Matrix.CreateRotationX(0.05f));
            if (ks.IsKeyDown(Keys.Left))
                _cone.Rotation *= Quaternion.CreateFromRotationMatrix(Matrix.CreateRotationY(-0.05f));
            if (ks.IsKeyDown(Keys.Right))
                _cone.Rotation *= Quaternion.CreateFromRotationMatrix(Matrix.CreateRotationY(0.05f));

            if (ks.IsKeyDown(Keys.W))
                _camera.Position += Vector3.Transform(new Vector3(0f, 0f, -0.1f), _camera.Rotation);
            if (ks.IsKeyDown(Keys.S))
                _camera.Position += Vector3.Transform(new Vector3(0f, 0f, 0.1f), _camera.Rotation);
            if (ks.IsKeyDown(Keys.A))
                _camera.Rotation *= Quaternion.CreateFromRotationMatrix(Matrix.CreateFromYawPitchRoll(0f, 0f, 0.01f));
            if (ks.IsKeyDown(Keys.D))
                _camera.Rotation *= Quaternion.CreateFromRotationMatrix(Matrix.CreateFromYawPitchRoll(0f, 0f, -0.01f));

            Vector2 center = new(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
            Vector2 mouseDelta = new(center.X - ms.X, center.Y - ms.Y);
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (mouseDelta != Vector2.Zero)
            {
                Debug.WriteLine(mouseDelta);
                Mouse.SetPosition(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
                _camera.Rotation *= Quaternion.CreateFromRotationMatrix(Matrix.CreateFromYawPitchRoll(MathHelper.ToRadians(mouseDelta.X * deltaTime * 20f), MathHelper.ToRadians(mouseDelta.Y * deltaTime * 20f), 0f));
            }
            _camera.Update(gameTime);

            if (ks.IsKeyDown(Keys.Space))
            {
                Vector3 size = Vector3.One / 10f;
                Vector3 position = new(_rng.NextFloat(-4f, 3f), _rng.NextFloat(-4f, 3f), _rng.NextFloat(-4f, 3f));
                BoundingBox box = new(position, position + size);
                BoxCollider col = new(box);
                _colliders.Add(col);
            }

            _octree.Clear();
            foreach (BoxCollider collider in _colliders)
                _octree.Add(collider);

            _lastKeyboard = ks;
            _lastMouse = ms;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _cone.Draw(GraphicsDevice, _camera.View);
            _octree.Draw(GraphicsDevice, _camera.View);
            base.Draw(gameTime);
        }
    }
}