using System.Collections.Generic;
using Alensia.Core.Camera;
using Alensia.Core.Input;
using Alensia.Core.Input.Generic;
using Alensia.Core.Locomotion;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace Alensia.Core.Control
{
    public class PlayerMovementControl : LocomotionControl<IWalkingLocomotion>
    {
        public const string Id = "Locomotion";

        public IBindingKey<IAxisInput> Horizontal => Keys.Horizontal;

        public IBindingKey<IAxisInput> Vertical => Keys.Vertical;

        public IBindingKey<TriggerStateInput> HoldToRun => Keys.HoldToRun;

        public readonly ICameraManager CameraManager;

        public Pacing WalkingPace = Pacing.Walking();

        public Pacing RunningPace = Pacing.Running();

        protected IAxisInput X { get; private set; }

        protected IAxisInput Y { get; private set; }

        protected TriggerStateInput Running { get; private set; }

        public override bool Valid => base.Valid &&
                                      Locomotion.Active &&
                                      X != null &&
                                      Y != null &&
                                      Running != null;

        public PlayerMovementControl(
            IWalkingLocomotion locomotion,
            ICameraManager cameraManager,
            IInputManager inputManager) : base(locomotion, inputManager)
        {
            Assert.IsNotNull(cameraManager, "cameraManager != null");

            CameraManager = cameraManager;
        }

        protected override ICollection<IBindingKey> PrepareBindings()
        {
            return new List<IBindingKey> {Keys.Horizontal, Keys.Vertical, Keys.HoldToRun};
        }

        protected override void OnBindingChange(IBindingKey key)
        {
            base.OnBindingChange(key);

            if (Equals(key, Keys.Horizontal))
            {
                X = InputManager.Get(Keys.Horizontal);
            }

            if (Equals(key, Keys.Vertical))
            {
                Y = InputManager.Get(Keys.Vertical);
            }

            if (Equals(key, Keys.HoldToRun))
            {
                Running = InputManager.Get(Keys.HoldToRun);
            }
        }

        protected override void OnActivate()
        {
            base.OnActivate();

            var direction = Observable
                .Zip(X.Value, Y.Value, Running.Value)
                .Select(xs => Tuple.Create(new Vector2(xs[0], xs[1]).normalized, xs[2]));

            Subsribe(direction, r => OnMove(r.Item1, r.Item2 > 0));
        }

        protected virtual void OnMove(Vector2 input, bool running)
        {
            var camera = CameraManager.Mode as IRotatableCamera;

            if (input.magnitude > 0 && camera is IPerspectiveCamera)
            {
                var speed = Locomotion.RotateTowards(Vector3.up, camera.Heading);

                camera.Heading -= speed * Time.deltaTime;
            }

            var movement = input.normalized;

            Locomotion.Move(new Vector3(movement.x, 0, movement.y));

            if (running && Locomotion.Pacing != RunningPace)
            {
                Locomotion.Pacing = RunningPace;
            }

            if (!running && Locomotion.Pacing == RunningPace)
            {
                Locomotion.Pacing = WalkingPace;
            }
        }

        public static class Keys
        {
            public static IBindingKey<IAxisInput> Horizontal =
                new BindingKey<IAxisInput>(Id + ".Horizontal");

            public static IBindingKey<IAxisInput> Vertical =
                new BindingKey<IAxisInput>(Id + ".Vertical");

            public static IBindingKey<TriggerStateInput> HoldToRun =
                new BindingKey<TriggerStateInput>(Id + ".HoldToRun");
        }
    }
}