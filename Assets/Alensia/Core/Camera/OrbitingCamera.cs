using Alensia.Core.Common;
using Alensia.Core.Geom;
using UniRx;
using UnityEngine;
using Zenject;

namespace Alensia.Core.Camera
{
    public abstract class OrbitingCamera : RotatableCamera, IOrbitingCamera, ILateTickable
    {
        public abstract DistanceSettings DistanceSettings { get; }

        public override float Heading
        {
            get { return _heading.Value; }
            set
            {
                _heading.Value = Mathf.Clamp(
                    GeometryUtils.NormalizeAspectAngle(value),
                    -RotationalConstraints.Side,
                    RotationalConstraints.Side);
            }
        }

        public override float Elevation
        {
            get { return _elevation.Value; }
            set
            {
                _elevation.Value = Mathf.Clamp(
                    GeometryUtils.NormalizeAspectAngle(value),
                    -RotationalConstraints.Down,
                    RotationalConstraints.Up);
            }
        }

        public float Distance
        {
            get { return _distance.Value; }
            set
            {
                _distance.Value = Mathf.Clamp(
                    value,
                    DistanceSettings.Minimum,
                    DistanceSettings.Maximum);
            }
        }

        private readonly IReactiveProperty<float> _heading;

        private readonly IReactiveProperty<float> _elevation;

        private readonly IReactiveProperty<float> _distance;

        protected OrbitingCamera(
            UnityEngine.Camera camera) : base(camera)
        {
            _heading = new ReactiveProperty<float>();
            _elevation = new ReactiveProperty<float>();
            _distance = new ReactiveProperty<float>();

            Observable
                .Zip(_heading, _elevation, _distance)
                .Where(_ => Valid && Active)
                .Subscribe(args => UpdatePosition(args[0], args[1], args[2]))
                .AddTo(this);
        }

        public override void Reset()
        {
            base.Reset();

            Distance = DistanceSettings.Default;
        }

        protected virtual void UpdatePosition(
            float heading, float elevation, float distance)
        {
            Transform.rotation = Quaternion.LookRotation(AxisForward, AxisUp);
            Transform.position = Pivot - AxisForward * distance;

            Transform.RotateAround(Pivot, Transform.right, -elevation);

            if (Mathf.Approximately(Mathf.Abs(elevation), 90))
            {
                Transform.Rotate(Vector3.forward, -heading);
            }
            else
            {
                Transform.RotateAround(Pivot, AxisUp, heading);
                Transform.LookAt(Pivot, AxisUp);
            }
        }

        public virtual void LateTick()
        {
            if (Valid && Active) UpdatePosition(Heading, Elevation, Distance);
        }
    }
}