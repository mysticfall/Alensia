using System;
using System.Collections.Generic;
using Alensia.Core.Input;
using Alensia.Core.Input.Generic;
using UniRx;

namespace Alensia.Core.Camera
{
    public class OrbitingCameraControl : RotatableCameraControl
    {
        public IBindingKey<IAxisInput> Zoom
        {
            get { return Keys.Zoom; }
        }

        protected IAxisInput Scroll { get; private set; }

        public override bool Valid
        {
            get { return base.Valid && Scroll != null; }
        }

        public OrbitingCameraControl(
            ICameraManager cameraManager,
            IInputManager inputManager) : base(cameraManager, inputManager)
        {
        }

        protected override ICollection<IBindingKey> PrepareBindings()
        {
            return new List<IBindingKey>(base.PrepareBindings()) {Zoom};
        }

        protected override void OnBindingChange(IBindingKey key)
        {
            base.OnBindingChange(key);

            if (Equals(key, Keys.Zoom))
            {
                Scroll = InputManager.Get(Keys.Zoom);
            }
        }

        protected override void OnActivate()
        {
            base.OnActivate();

            var zoom = Scroll.Value
                .Where(_ => CameraManager.Mode is IZoomableCamera)
                .Select(v => (float) -Math.Sign(v));

            Subsribe(zoom, OnZoom);
        }

        protected void OnZoom(float input)
        {
            OnZoom(input, (IZoomableCamera) CameraManager.Mode);
        }

        protected virtual void OnZoom(float input, IZoomableCamera camera)
        {
            camera.Distance += input;
        }

        public new class Keys : RotatableCameraControl.Keys
        {
            public static IBindingKey<IAxisInput> Zoom = new BindingKey<IAxisInput>(Id + ".Zoom");
        }
    }
}