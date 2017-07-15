using System;
using System.Collections.Generic;
using Alensia.Core.Input;
using Alensia.Core.Input.Generic;
using UniRx;
using UnityEngine;

namespace Alensia.Core.Camera
{
    public class RotatableCameraControl : CameraControl
    {
        public const string Id = "Camera";

        public override string Name => Id;

        public IBindingKey<IAxisInput> Yaw => Keys.Yaw;

        public IBindingKey<IAxisInput> Pitch => Keys.Pitch;

        protected IAxisInput X { get; private set; }

        protected IAxisInput Y { get; private set; }

        public override bool Valid => base.Valid && X != null && Y != null;

        public RotatableCameraControl(
            ICameraManager cameraManager,
            IInputManager inputManager) : base(cameraManager, inputManager)
        {
        }

        protected override bool Supports(ICameraMode camera) =>
            base.Supports(camera) && camera is IRotatableCamera;

        protected override ICollection<IBindingKey> PrepareBindings() => 
            new List<IBindingKey> {Yaw, Pitch};

        protected override void RegisterDefaultBindings()
        {
            base.RegisterDefaultBindings();

            InputManager.Register(Yaw, new AxisInput("Mouse X"));
            InputManager.Register(Pitch, new AxisInput("Mouse Y"));
        }

        protected override void OnBindingChange(IBindingKey key)
        {
            base.OnBindingChange(key);

            if (Equals(key, Yaw))
            {
                X = InputManager.Get(Yaw);
            }

            if (Equals(key, Pitch))
            {
                Y = InputManager.Get(Pitch);
            }
        }

        protected override void Subscribe(ICollection<IDisposable> disposables)
        {
            Observable
                .Zip(X.OnChange, Y.OnChange)
                .Where(_ => Valid)
                .Select(xs => new Vector2(xs[0], xs[1]))
                .Subscribe(OnRotate)
                .AddTo(disposables);
        }

        protected void OnRotate(Vector2 input) => OnRotate(input, (IRotatableCamera) CameraManager.Mode);

        protected virtual void OnRotate(Vector2 input, IRotatableCamera camera)
        {
            camera.Heading += input.x;
            camera.Elevation += input.y;
        }

        public class Keys
        {
            public static IBindingKey<IAxisInput> Yaw = new BindingKey<IAxisInput>(Id + ".Yaw");

            public static IBindingKey<IAxisInput> Pitch = new BindingKey<IAxisInput>(Id + ".Pitch");
        }
    }
}