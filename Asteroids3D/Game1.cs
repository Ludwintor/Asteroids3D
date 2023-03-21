using System;
using System.Collections.Generic;
using Asteroids3D.Core;
using Asteroids3D.Mesh;
using Asteroids3D.Physics;
using Asteroids3D.Player;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX;
using BoundingBox = Microsoft.Xna.Framework.BoundingBox;
using Color = Microsoft.Xna.Framework.Color;
using Quaternion = Microsoft.Xna.Framework.Quaternion;
using Vector3 = Microsoft.Xna.Framework.Vector3;

namespace Asteroids3D
{
    public class Game1 : Game
    {
        public BasicEffect DefaultFX { get; private set; }
        public Camera MainCamera { get; private set; }

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private KeyboardState _lastKeyboard;

        private PlayerController _player;
        private PhysicsWorld _physics;
        private Random _rng;
        private List<Asteroid> _asteroids;
        private int _frame;
        private bool _renderOctree;
        private Border _border;

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
            _asteroids = new List<Asteroid>();
            _physics = new PhysicsWorld(new BoundingBox(Vector3.One * -25f, Vector3.One * 25f));
            MainCamera = new Camera(this)
            {
                Position = new Vector3(0f, 1.2f, 5f),
                Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathHelper.ToRadians(65f))
            };
            CameraFollow follow = new(MainCamera);
            DefaultFX = new BasicEffect(GraphicsDevice)
            {
                VertexColorEnabled = true,
                Projection = MainCamera.Projection
            };
            _player = new PlayerController(this, follow)
            {
                MeshRenderer = new MeshRenderer
                {
                    Mesh = new ConeMesh(GraphicsDevice, 1f, 0.5f, Color.Green, Color.Red, 20),
                    Effect = DefaultFX
                }
            };
            _player.Transform.Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitX, -MathHelper.PiOver2);
            _physics.AddCollider(_player.Collider);
            _border = new Border(new BoundingBox(Vector3.One * -25f, Vector3.One * 25f), _player);
            _rng = new Random();
            Gizmos.Init(GraphicsDevice, MainCamera);


            for (int i = 0; i < 30; i++)
            {
                Vector3 position = new(_rng.NextFloat(-24f, 24f), _rng.NextFloat(-24f, 24f), _rng.NextFloat(-24f, 24f));
                Asteroid asteroid = new(this);
                asteroid.Transform.Position = position;
                asteroid.Destroyed += OnAsteroidDestroyed;
                _physics.AddCollider(asteroid.Collider);
                _asteroids.Add(asteroid);
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
            _frame++;
            if (ks.IsKeyDown(Keys.Escape))
                Exit();

            if (ks.IsKeyDown(Keys.Up))
                _player.Transform.Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitX, -0.05f) * _player.Transform.Rotation;
            if (ks.IsKeyDown(Keys.Down))
                _player.Transform.Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitX, 0.05f) * _player.Transform.Rotation;
            if (ks.IsKeyDown(Keys.Left))
                _player.Transform.Rotation = Quaternion.CreateFromAxisAngle(-Vector3.UnitZ, -0.05f) * _player.Transform.Rotation;
            if (ks.IsKeyDown(Keys.Right))
                _player.Transform.Rotation = Quaternion.CreateFromAxisAngle(-Vector3.UnitZ, 0.05f) * _player.Transform.Rotation;
            _player.Update(gameTime, _frame, new(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2));
            _border.Update(gameTime);

            if (ks.IsKeyDown(Keys.Space))
            {
                Vector3 position = new(_rng.NextFloat(-24f, 24f), _rng.NextFloat(-24f, 24f), _rng.NextFloat(-24f, 24f));
                Asteroid asteroid = new(this);
                asteroid.Transform.Position = position;
                asteroid.Destroyed += OnAsteroidDestroyed;
                _physics.AddCollider(asteroid.Collider);
                _asteroids.Add(asteroid);
            }

            if (!_lastKeyboard.IsKeyDown(Keys.O) && ks.IsKeyDown(Keys.O))
                _renderOctree = !_renderOctree;

            _physics.Update(gameTime);

            _lastKeyboard = ks;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _player.Draw();
            _border.Draw();
            foreach (Asteroid asteroid in _asteroids)
                asteroid.Draw();
            if (_renderOctree)
                _physics.Draw();
            base.Draw(gameTime);
        }

        private void OnAsteroidDestroyed(Asteroid asteroid)
        {
            _asteroids.Remove(asteroid);
            _physics.RemoveCollider(asteroid.Collider);
            asteroid.Destroyed -= OnAsteroidDestroyed;
        }
    }
}