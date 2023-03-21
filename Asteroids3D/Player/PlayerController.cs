using Asteroids3D.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Asteroids3D.Player
{
    public class PlayerController : GameObject
    {
        private CameraFollow _follow;

        public PlayerController(Game1 game, CameraFollow follow) : base(game)
        {
            follow.Target = Transform;
            _follow = follow;
            Collider = new SphereCollider(Transform, 0.65f);
            Collider.Collided += OnCollision;
        }

        public void Update(GameTime time, int frame, Vector2 center)
        {
            KeyboardState ks = Keyboard.GetState();
            MouseState ms = Mouse.GetState();
            Quaternion piOver2 = Quaternion.CreateFromAxisAngle(Vector3.UnitX, -MathHelper.PiOver2);
            if (ks.IsKeyDown(Keys.W))
                Transform.Position += Vector3.Transform(new Vector3(0f, 0f, 0.1f), Transform.Rotation * piOver2);
            if (ks.IsKeyDown(Keys.S))
                Transform.Position += Vector3.Transform(new Vector3(0f, 0f, -0.1f), Transform.Rotation * piOver2);
            if (ks.IsKeyDown(Keys.A))
                Transform.Rotation *= Quaternion.CreateFromAxisAngle(Vector3.Transform(-Vector3.UnitZ, -_follow.InitialRotation), -0.01f);
            if (ks.IsKeyDown(Keys.D))
                Transform.Rotation *= Quaternion.CreateFromAxisAngle(Vector3.Transform(-Vector3.UnitZ, -_follow.InitialRotation), 0.01f);

            Vector2 mouseDelta = new((center.X - ms.X) / center.X * 2f, (center.Y - ms.Y) / center.Y * 2f);
            float deltaTime = (float)time.ElapsedGameTime.TotalSeconds;
            if (mouseDelta != Vector2.Zero)
            {
                if (frame % 4 == 0)
                {
                    Mouse.SetPosition((int)center.X, (int)center.Y);
                }
                Vector2 mouseMove = new Vector2(MathHelper.ToRadians(mouseDelta.X), MathHelper.ToRadians(mouseDelta.Y)) * deltaTime * 7000f;
                Transform.Rotation *= Quaternion.CreateFromAxisAngle(Vector3.Transform(Vector3.UnitY, piOver2), -mouseMove.X) *
                                      Quaternion.CreateFromAxisAngle(Vector3.Transform(Vector3.UnitX, piOver2), mouseMove.Y);
            }

            _follow.Update(time);
        }

        private void OnCollision(CollisionInfo info)
        {

        }
    }
}
