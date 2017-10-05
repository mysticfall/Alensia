using Alensia.Core.Character;
using UnityEngine;

namespace Alensia.Core.Camera
{
    public class CharacterCamera : OrbitingCamera, ITrackingCamera<IHumanoid>
    {
        public override RotationalConstraints RotationalConstraints => _rotation;

        public override DistanceSettings DistanceSettings => _distance;

        public override bool Valid => base.Valid && Target != null;

        public IHumanoid Target { get; private set; }

        public Transform BodyPart { get; private set; }

        public Vector3 CameraOffset
        {
            get { return _cameraOffset; }
            set { _cameraOffset = value; }
        }

        public override Vector3 Pivot => BodyPart.position +
                                         AxisUp * CameraOffset.y +
                                         AxisRight * CameraOffset.x +
                                         AxisForward * CameraOffset.z;

        public override Vector3 AxisForward => Target.Transform.forward * -1;

        public override Vector3 AxisUp => Target.Transform.up;

        [SerializeField] private RotationalConstraints _rotation;

        [SerializeField] private DistanceSettings _distance;

        [SerializeField] private Vector3 _cameraOffset;

        public CharacterCamera()
        {
            _rotation = new RotationalConstraints
            {
                Down = 80,
                Side = 180,
                Up = 80
            };

            _distance = new DistanceSettings
            {
                Minimum = 0.2f,
                Maximum = 2f
            };
        }

        public virtual void Track(IHumanoid target)
        {
            Target = target;
            Distance = DistanceSettings.Default;

            if (target != null)
            {
                Focus(HumanBodyBones.Chest);
            }
        }

        public virtual void Focus(HumanBodyBones focus)
        {
            BodyPart = Target?.GetBodyPart(focus);
        }
    }
}