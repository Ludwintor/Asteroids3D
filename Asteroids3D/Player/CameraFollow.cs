using Asteroids3D.Core;
using Microsoft.Xna.Framework;

namespace Asteroids3D.Player
{
    public class CameraFollow
    {
        public Vector3 InitialPosition { get; private set; }

        public Quaternion InitialRotation { get; private set; }

        private Camera _camera;

        public Transform Target { get; set; }

        public CameraFollow(Camera camera)
        {
            Init(camera);
        }

        public void Init(Camera camera)
        {
            _camera = camera;
            InitialPosition = camera.Position;
            InitialRotation = camera.Rotation;
        }

        public void Update(GameTime time)
        {
            _camera.Rotation = Target.Rotation * InitialRotation;
            _camera.Position = Vector3.Transform(InitialPosition, _camera.Rotation) + Target.Position;
            _camera.Update(time);
        }
    }
}
