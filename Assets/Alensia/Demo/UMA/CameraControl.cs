using System;
using Alensia.Core.Camera;
using Alensia.Core.UI;
using Alensia.Core.UI.Event;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace Alensia.Demo.UMA
{
    public class CameraControl : UIHandler<Panel>
    {
        [Inject, NonSerialized] public ICameraManager CameraManager;

        public CharacterCamera Camera { get; private set; }

        public float InitialZoom { get; private set; }

        [Range(0.1f, 1f)] public float Sensitivity = 0.7f;

        public DragButton RotateButton;

        public DragButton MoveButton;

        public Button ResetButton;

        public Slider ZoomSlider;

        private bool _dragFinished;

        public override void Initialize(IUIContext context)
        {
            base.Initialize(context);

            Camera = CameraManager.Mode as CharacterCamera;

            Assert.IsNotNull(Camera, "Camera != null");

            Action<IPointerDragAware, Vector3> register = (button, direction) =>
            {
                var events = button.OnDrag.Select(v => v.position);

                Observable.Zip(events, events.Skip(1))
                    .Select(v => v[1] - v[0])
                    .Select(v => Normalize(v, direction))
                    .Subscribe(MoveCamera)
                    .AddTo(this);

                button.OnDragEnd
                    .Subscribe(_ => _dragFinished = true)
                    .AddTo(this);
            };

            register(RotateButton, Vector3.right);
            register(MoveButton, Vector3.up);

            ResetButton.OnPointerSelect
                .Subscribe(_ => ResetCamera())
                .AddTo(this);

            var max = Camera.DistanceSettings.Maximum;
            var min = Camera.DistanceSettings.Minimum;

            var diff = max - min;

            var zoom = Mathf.Approximately(diff, 0) ? 0 : (Camera.Distance - min) / diff;

            InitialZoom = Mathf.Clamp(zoom, min, max);

            ZoomSlider.Value = InitialZoom;
            ZoomSlider
                .OnValueChange
                .Subscribe(ZoomCamera)
                .AddTo(this);
        }

        private void MoveCamera(Vector2 delta)
        {
            if (_dragFinished)
            {
                _dragFinished = false;
                return;
            }

            var offset = new Vector3(0, delta.y * 0.02f * Sensitivity, 0);

            Camera.Heading += delta.x * Sensitivity;
            Camera.CameraOffset = Camera.CameraOffset + offset;
        }

        private void ZoomCamera(float zoom)
        {
            var max = Camera.DistanceSettings.Maximum;
            var min = Camera.DistanceSettings.Minimum;

            Camera.Distance = (ZoomSlider.MaxValue - zoom) * (max - min) + min;
        }

        private void ResetCamera()
        {
            Camera.Heading = 0;
            Camera.CameraOffset = Vector3.zero;

            ZoomSlider.Value = InitialZoom;
        }

        private static Vector2 Normalize(Vector2 vector, Vector2 direction)
        {
            vector.Scale(direction);
            return vector;
        }
    }
}