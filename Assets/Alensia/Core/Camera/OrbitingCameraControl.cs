using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Alensia.Core.Input;
using Alensia.Core.Input.Generic;
using UniRx;
using UnityEngine;

namespace Alensia.Core.Camera
{
    public class OrbitingCameraControl : CameraControl
    {
        public virtual IBindingKey<IAxisInput> Yaw => Keys.Yaw;

        public virtual IBindingKey<IAxisInput> Pitch => Keys.Pitch;

        public virtual IBindingKey<IAxisInput> Zoom => Keys.Zoom;

        protected IAxisInput X { get; private set; }

        protected IAxisInput Y { get; private set; }

        protected IAxisInput Scroll { get; private set; }

        public override bool Valid => base.Valid && X != null && Y != null && Scroll != null;

        protected override bool Supports(ICameraMode mode) => mode is IOrbitingCamera;

        protected override IEnumerable<IBindingKey> PrepareBindings() => new List<IBindingKey> {Yaw, Pitch, Zoom};

        protected override void RegisterDefaultBindings()
        {
            base.RegisterDefaultBindings();

            InputManager.Register(Yaw, new AxisInput("Mouse X"));
            InputManager.Register(Pitch, new AxisInput("Mouse Y"));
            InputManager.Register(Zoom, new AxisInput("Mouse ScrollWheel", 0.15f));
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

            if (Equals(key, Zoom))
            {
                Scroll = InputManager.Get(Zoom);
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

            Scroll.OnChange
                .Where(_ => Valid)
                .Select(v => v * -15)
                .Subscribe(OnZoom)
                .AddTo(disposables);
        }

        protected void OnRotate(Vector2 input) => OnRotate(input, (IRotatableCamera) CameraManager.Mode);

        protected virtual void OnRotate(Vector2 input, IRotatableCamera mode)
        {
            mode.Heading += input.x * Sensitivity.Horizontal;
            mode.Elevation += input.y * Sensitivity.Vertical;
        }

        protected virtual void OnZoom(float input) => OnZoom(input, (IZoomableCamera) CameraManager.Mode);

        protected virtual void OnZoom(float input, IZoomableCamera mode)
        {
            mode.Distance += input * Sensitivity.Zoom;
        }

        [SuppressMessage("ReSharper", "FieldCanBeMadeReadOnly.Global")]
        public class Keys
        {
            public static IBindingKey<IAxisInput> Yaw = new BindingKey<IAxisInput>(Category + ".Yaw");

            public static IBindingKey<IAxisInput> Pitch = new BindingKey<IAxisInput>(Category + ".Pitch");

            public static IBindingKey<IAxisInput> Zoom = new BindingKey<IAxisInput>(Category + ".Zoom");
        }
    }
}