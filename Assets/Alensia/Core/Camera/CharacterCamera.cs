using System;
using Alensia.Core.Character;
using Alensia.Core.Common;
using UnityEngine;
using Zenject;

namespace Alensia.Core.Camera
{
    public class CharacterCamera : OrbitingCamera, ITrackingCamera<IHumanoid>
    {
        public override RotationalConstraints RotationalConstraints => _settings.Rotation;

        public override DistanceSettings DistanceSettings => _settings.Distance;

        public override bool Valid => base.Valid && Target != null;

        public IHumanoid Target { get; private set; }

        public Transform BodyPart { get; private set; }

        public Vector3 CameraOffset { get; set; }

        public override Vector3 Pivot => BodyPart.position + AxisUp * CameraOffset.y;

        public override Vector3 AxisForward => Target.Transform.forward * -1;

        public override Vector3 AxisUp => Target.Transform.up;

        private readonly Settings _settings;

        public CharacterCamera(UnityEngine.Camera camera) : this(null, camera)
        {
        }

        [Inject]
        public CharacterCamera(
            [InjectOptional] Settings settings,
            UnityEngine.Camera camera) : base(camera)
        {
            _settings = settings ?? new Settings();

            CameraOffset = _settings.CameraOffset;
        }

        public virtual void Track(IHumanoid target)
        {
            Target = target;
            Distance = DistanceSettings.Default;

            BodyPart = Target?.GetBodyPart(HumanBodyBones.Chest);
        }


        [Serializable]
        public class Settings : IEditorSettings
        {
            public RotationalConstraints Rotation = new RotationalConstraints
            {
                Down = 80,
                Side = 180,
                Up = 80
            };

            public DistanceSettings Distance = new DistanceSettings
            {
                Minimum = 0.2f,
                Maximum = 2f
            };

            public Vector3 CameraOffset;
        }
    }
}