using System;
using System.Collections.Generic;
using Asteroids3D.Player;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX;
using BoundingBox = Microsoft.Xna.Framework.BoundingBox;
using Color = Microsoft.Xna.Framework.Color;
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
        private PlayerController _player;
        private Octree _octree;
        private Random _rng;
        private Camera _camera;
        private List<BoxCollider> _colliders;
        private int _frame;

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
                Position = new Vector3(0f, 1.2f, 5f),
                Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathHelper.ToRadians(70f))
            };
            CameraFollow follow = new(_camera);
            _fx = new BasicEffect(GraphicsDevice)
            {
                VertexColorEnabled = true,
                Projection = _camera.Projection
            };
            _player = new PlayerController(follow)
            {
                Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitX, -MathHelper.PiOver2),
                MeshRenderer = new MeshRenderer
                {
                    Mesh = new ConeMesh(GraphicsDevice, 1f, 0.5f, Color.Green, Color.Red, 30),
                    Effect = _fx
                }
            };
            _octree = new Octree(new BoundingBox(Vector3.One * -5f, Vector3.One * 5f));
            _rng = new Random();
            Gizmos.Init(GraphicsDevice, _camera);


            for (int i = 0; i < 30; i++)
            {
                Vector3 size = Vector3.One / 10f;
                Vector3 position = new(_rng.NextFloat(-4f, 4f), _rng.NextFloat(-4f, 4f), _rng.NextFloat(-4f, 4f));
                BoundingBox box = new(position, position + size);
                BoxCollider col = new(box);
                _colliders.Add(col);
            }

            Mouse.SetPosition(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);

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
            _frame++;
            if (ks.IsKeyDown(Keys.Escape))
                Exit();

            if (ks.IsKeyDown(Keys.Up))
                _player.Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitX, -0.05f) * _player.Rotation;
            if (ks.IsKeyDown(Keys.Down))
                _player.Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitX, 0.05f) * _player.Rotation;
            if (ks.IsKeyDown(Keys.Left))
                _player.Rotation = Quaternion.CreateFromAxisAngle(-Vector3.UnitZ, -0.05f) * _player.Rotation;
            if (ks.IsKeyDown(Keys.Right))
                _player.Rotation = Quaternion.CreateFromAxisAngle(-Vector3.UnitZ, 0.05f) * _player.Rotation;
            _player.Update(gameTime, _frame, new(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2));

            if (ks.IsKeyDown(Keys.Space))
            {
                Vector3 size = Vector3.One / 10f;
                Vector3 position = new(_rng.NextFloat(-4f, 4f), _rng.NextFloat(-4f, 4f), _rng.NextFloat(-4f, 4f));
                BoundingBox box = new(position, position + size);
                BoxCollider col = new(box);
                _colliders.Add(col);
            }

            _octree.Clear();
            foreach (BoxCollider collider in _colliders)
                _octree.Add(collider);

            _lastKeyboard = ks;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _player.Draw(GraphicsDevice, _camera.View);
            _octree.Draw(GraphicsDevice, _camera.View);
            foreach (BoxCollider collider in _colliders)
                collider.Draw(GraphicsDevice, _camera.View);
            base.Draw(gameTime);
        }
    }
}