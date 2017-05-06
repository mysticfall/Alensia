using System.Collections.Generic;
using Alensia.Core.Input;
using Alensia.Core.Input.Generic;
using UniRx;
using UnityEngine;

namespace Alensia.Core.Control
{
    public abstract class Vector2Control : Control<Vector2>
    {
        public abstract IBindingKey<IAxisInput> X { get; }

        public abstract IBindingKey<IAxisInput> Y { get; }

        protected IAxisInput XInput { get; private set; }

        protected IAxisInput YInput { get; private set; }

        public override bool Valid
        {
            get { return base.Valid && XInput != null && YInput != null; }
        }

        protected Vector2Control(IInputManager inputManager) : base(inputManager)
        {
        }

        protected override ICollection<IBindingKey> PrepareBindings()
        {
            return new List<IBindingKey> {X, Y};
        }

        protected override void OnBindingChange(IBindingKey key)
        {
            base.OnBindingChange(key);

            if (Equals(key, X))
            {
                XInput = InputManager.Get(X);
            }

            if (Equals(key, Y))
            {
                YInput = InputManager.Get(Y);
            }
        }

        protected override IObservable<Vector2> Observe()
        {
            return Observable
                .Zip(XInput.Value, YInput.Value)
                .Select(xs => new Vector2(xs[0], xs[1]));
        }
    }
}