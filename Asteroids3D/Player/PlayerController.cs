using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Asteroids3D.Player
{
    public class PlayerController : GameObject
    {
        private CameraFollow _follow;

        public PlayerController(CameraFollow follow)
        {
            follow.Target = this;
            _follow = follow;
        }

        public void Update(GameTime time, int frame, Vector2 center)
        {
            KeyboardState ks = Keyboard.GetState();
            MouseState ms = Mouse.GetState();
            if (ks.IsKeyDown(Keys.W))
                Position += Vector3.Transform(new Vector3(0f, 0f, -0.1f), Rotation * -_follow.InitialRotation);
            if (ks.IsKeyDown(Keys.S))
                Position += Vector3.Transform(new Vector3(0f, 0f, 0.1f), Rotation * -_follow.InitialRotation);
            if (ks.IsKeyDown(Keys.A))
                Rotation *= Quaternion.CreateFromAxisAngle(Vector3.Transform(-Vector3.UnitZ, -_follow.InitialRotation), -0.01f);
            if (ks.IsKeyDown(Keys.D))
                Rotation *= Quaternion.CreateFromAxisAngle(Vector3.Transform(-Vector3.UnitZ, -_follow.InitialRotation), 0.01f);

            Vector2 mouseDelta = new((center.X - ms.X) / center.X * 2f, (center.Y - ms.Y) / center.Y * 2f);
            float deltaTime = (float)time.ElapsedGameTime.TotalSeconds;
            if (mouseDelta != Vector2.Zero)
            {
                if (frame % 4 == 0)
                {
                    Mouse.SetPosition((int)center.X, (int)center.Y);
                }
                //Rotation *= Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(mouseDelta.X * deltaTime * 8000f), MathHelper.ToRadians(mouseDelta.Y * deltaTime * 8000f), 0f);
                Rotation *= Quaternion.CreateFromAxisAngle(Vector3.Transform(Vector3.UnitY, -_follow.InitialRotation), MathHelper.ToRadians(mouseDelta.X * deltaTime * 8000f)) *
                            Quaternion.CreateFromAxisAngle(Vector3.Transform(Vector3.UnitX, -_follow.InitialRotation), MathHelper.ToRadians(mouseDelta.Y * deltaTime * 8000f));
            }

            _follow.Update(time);
        }
    }
}
