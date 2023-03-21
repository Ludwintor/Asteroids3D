using Microsoft.Xna.Framework;

namespace Asteroids3D
{
    public class Camera : GameComponent
    {
        private Vector3 _cameraLookAt;

        public Camera(Game game) : base(game)
        {
            Projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver2,
                game.GraphicsDevice.Viewport.AspectRatio, 0.1f, 1000f);
        }

        public Matrix Projection { get; }

        public Matrix View => Matrix.CreateLookAt(Position, _cameraLookAt, Vector3.Transform(Vector3.Up, Rotation));

        public Vector3 Position { get; set; } = Vector3.Zero;

        public Quaternion Rotation { get; set; } = Quaternion.Identity;

        public Matrix Transform => Matrix.CreateFromQuaternion(Rotation) * Matrix.CreateTranslation(Position);

        public override void Update(GameTime gameTime)
        {
            UpdateLookAt();
        }

        private void UpdateLookAt()
        {
            Vector3 lookAtOffset = Vector3.Transform(-Vector3.UnitZ, Rotation);
            _cameraLookAt = Position + lookAtOffset;
        }
    }
}
