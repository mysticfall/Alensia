using System;
using System.Collections.Generic;
using Alensia.Core.Camera;
using Alensia.Core.Character;
using Alensia.Core.Input;
using Alensia.Core.Input.Generic;
using Alensia.Core.Locomotion;
using Alensia.Core.UI.Cursor;
using UniRx;
using UnityEngine;
using Zenject;

namespace Alensia.Core.Control
{
    public class PlayerMovementControl : LocomotionControl<ILeggedLocomotion>, IPlayerControl
    {
        public IHumanoid Player { get; set; }

        public override ILeggedLocomotion Locomotion => Player?.Locomotion;

        public IBindingKey<IAxisInput> Horizontal => Keys.Horizontal;

        public IBindingKey<IAxisInput> Vertical => Keys.Vertical;

        public IBindingKey<TriggerStateInput> HoldToRun => Keys.HoldToRun;

        [Inject]
        public ICameraManager CameraManager { get; }

        public Pacing WalkingPace = Pacing.Walking();

        public Pacing RunningPace = Pacing.Running();

        protected IAxisInput X { get; private set; }

        protected IAxisInput Y { get; private set; }

        protected TriggerStateInput Running { get; private set; }

        public override CursorState CursorState => CursorState.Hidden;

        public override bool Valid => base.Valid &&
                                      X != null &&
                                      Y != null &&
                                      Running != null &&
                                      Locomotion.Active;

        protected override ICollection<IBindingKey> PrepareBindings() =>
            new List<IBindingKey> {Horizontal, Vertical, HoldToRun};

        protected override void RegisterDefaultBindings()
        {
            base.RegisterDefaultBindings();

            InputManager.Register(Horizontal, new AxisInput("Horizontal"));
            InputManager.Register(Vertical, new AxisInput("Vertical"));
            InputManager.Register(HoldToRun, new TriggerStateInput(new KeyTrigger(KeyCode.LeftShift)));
        }

        protected override void OnBindingChange(IBindingKey key)
        {
            base.OnBindingChange(key);

            if (Equals(key, Horizontal))
            {
                X = InputManager.Get(Horizontal);
            }

            if (Equals(key, Vertical))
            {
                Y = InputManager.Get(Vertical);
            }

            if (Equals(key, HoldToRun))
            {
                Running = InputManager.Get(HoldToRun);
            }
        }

        protected override void Subscribe(ICollection<IDisposable> disposables)
        {
            Observable
                .Zip(X.OnChange, Y.OnChange, Running.OnChange)
                .Where(_ => Valid)
                .Select(xs => Tuple.Create(new Vector2(xs[0], xs[1]).normalized, xs[2]))
                .Subscribe(r => OnMove(r.Item1, r.Item2 > 0), Debug.LogError)
                .AddTo(disposables);
        }

        protected virtual void OnMove(Vector2 input, bool running)
        {
            var cam = CameraManager.Mode as IRotatableCamera;

            if (input.magnitude > 0 && cam is IPerspectiveCamera)
            {
                var speed = Locomotion.RotateTowards(Vector3.up, cam.Heading);

                cam.Heading -= speed * Time.deltaTime;
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
                new BindingKey<IAxisInput>(Category + ".Horizontal");

            public static IBindingKey<IAxisInput> Vertical =
                new BindingKey<IAxisInput>(Category + ".Vertical");

            public static IBindingKey<TriggerStateInput> HoldToRun =
                new BindingKey<TriggerStateInput>(Category + ".HoldToRun");
        }
    }
}