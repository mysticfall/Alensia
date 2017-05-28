using System.Collections.Generic;
using Alensia.Core.Input;
using Alensia.Core.Input.Generic;
using UniRx;

namespace Alensia.Core.Camera
{
    public class OrbitingCameraControl : RotatableCameraControl
    {
        public IBindingKey<IAxisInput> Zoom => Keys.Zoom;

        protected IAxisInput Scroll { get; private set; }

        public override bool Valid => base.Valid && Scroll != null;

        public OrbitingCameraControl(
            ICameraManager cameraManager,
            IInputManager inputManager) : base(cameraManager, inputManager)
        {
        }

        protected override ICollection<IBindingKey> PrepareBindings()
        {
            return new List<IBindingKey>(base.PrepareBindings()) {Zoom};
        }

        protected override void RegisterDefaultBindings()
        {
            base.RegisterDefaultBindings();

            InputManager.Register(Keys.Zoom, new AxisInput("Mouse ScrollWheel", 0.15f));
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

            Scroll.Value
                .Where(_ => Active && Valid)
                .Where(_ => CameraManager.Mode is IZoomableCamera)
                .Select(v => v * -15)
                .Subscribe(OnZoom)
                .AddTo(Observers);
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