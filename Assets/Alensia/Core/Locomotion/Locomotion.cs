using System;
using Alensia.Core.Common;
using UniRx;
using UnityEngine;
using Zenject;

namespace Alensia.Core.Locomotion
{
    public abstract class Locomotion : ActivatableMonoBehavior, ITickable
    {
        [Inject]
        public Transform Transform { get; }

        public GameObject GameObject => Transform.gameObject;

        private Vector3 _targetVelocity;

        private Vector3 _targetAngularVelocity;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            OnInitialize
                .Subscribe(_ => Activate(), Debug.LogError)
                .AddTo(this);
            OnActiveStateChange
                .Subscribe(_ => Reset(), Debug.LogError)
                .AddTo(this);
        }

        public float Move(Vector3 direction)
        {
            if (!Active) return 0;

            _targetVelocity = CalculateVelocity(direction.normalized);

            return _targetVelocity.magnitude;
        }

        public float MoveTowards(Vector3 position)
        {
            if (!Active) return 0;

            var offset = position - Transform.localPosition;

            var direction = offset.normalized;
            var distance = offset.magnitude;

            _targetVelocity = CalculateVelocity(direction, distance);

            return _targetVelocity.magnitude;
        }

        public float Rotate(Vector3 axis)
        {
            if (!Active) return 0;

            _targetAngularVelocity = CalculateAngularVelocity(axis.normalized);

            return _targetAngularVelocity.magnitude;
        }

        public float RotateTowards(Vector3 axis, float degree)
        {
            if (!Active) return 0;

            _targetAngularVelocity = CalculateAngularVelocity(axis.normalized, degree);

            return _targetAngularVelocity.magnitude * Math.Sign(degree);
        }

        private void Reset()
        {
            _targetVelocity = Vector3.zero;
            _targetAngularVelocity = Vector3.zero;
        }

        public virtual void Tick()
        {
            if (!Active) return;

            UpdateVelocity(_targetVelocity, _targetAngularVelocity);

            Reset();
        }

        protected abstract Vector3 CalculateVelocity(Vector3 direction, float? distance = null);

        protected abstract Vector3 CalculateAngularVelocity(Vector3 axis, float? degrees = null);

        protected virtual void UpdateVelocity(Vector3 velocity, Vector3 angularVelocity)
        {
            UpdateVelocity(velocity);
            UpdateRotation(angularVelocity);
        }

        protected abstract void UpdateVelocity(Vector3 velocity);

        protected abstract void UpdateRotation(Vector3 angularVelocity);
    }
}