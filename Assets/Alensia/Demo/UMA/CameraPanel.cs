using System;
using System.Linq;
using Alensia.Core.Camera;
using Alensia.Core.Character;
using Alensia.Core.UI;
using Alensia.Core.UI.Event;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace Alensia.Demo.UMA
{
    public class CameraPanel : CharacterPanel
    {
        [Inject]
        public ICameraManager CameraManager { get; }

        public CharacterCamera Camera { get; private set; }

        public Animator Animator { get; private set; }

        public float InitialZoom { get; private set; }

        [Range(0.1f, 1f)] public float Sensitivity = 0.5f;

        public DragButton RotateButton;

        public DragButton MoveButton;

        public Button ResetButton;

        public Slider ZoomSlider;

        public ToggleButton AnimationToggle;

        public Panel FocusControlPanel;

        public override void Initialize(IUIContext context)
        {
            base.Initialize(context);

            Camera = CameraManager.Mode as CharacterCamera;

            Assert.IsNotNull(Camera, "Camera != null");

            Action<IPointerDragAware, Vector3> register = (button, direction) =>
            {
                var events = button.OnDrag.Select(v => v.position);

                Observable
                    .Zip(events, events.Skip(1))
                    .TakeUntil(button.OnDragEnd)
                    .RepeatSafe()
                    .Select(i => Normalize(i[1] - i[0], direction))
                    .Subscribe(MoveCamera)
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

            AnimationToggle.enabled = false;
            AnimationToggle.OnValueChange
                .Where(_ => Animator != null)
                .Subscribe(v => Animator.enabled = v)
                .AddTo(this);

            var focusButtons = FocusControlPanel.Children.Cast<Button>();

            foreach (var button in focusButtons)
            {
                button.OnPointerSelect
                    .Select(v => v.selectedObject.GetComponent<FocusTarget>())
                    .Subscribe(FocusCamera)
                    .AddTo(this);
            }
        }

        protected override void LoadCharacter(IHumanoid character)
        {
            base.LoadCharacter(character);

            Animator = character?.Animator;

            AnimationToggle.enabled = Animator != null;
            AnimationToggle.Value = Animator != null && Animator.enabled;
        }

        private void MoveCamera(Vector2 delta)
        {
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

        private void FocusCamera(FocusTarget target)
        {
            ResetCamera();

            ZoomSlider.Value = target.Zoom;

            Camera.Focus(target.Target);
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