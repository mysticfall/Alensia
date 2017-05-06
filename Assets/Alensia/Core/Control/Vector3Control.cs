using System.Collections.Generic;
using Alensia.Core.Input;
using Alensia.Core.Input.Generic;
using UniRx;
using UnityEngine;

namespace Alensia.Core.Control
{
    public abstract class Vector3Control : Control<Vector3>
    {
        public abstract IBindingKey<IAxisInput> X { get; }

        public abstract IBindingKey<IAxisInput> Y { get; }

        public abstract IBindingKey<IAxisInput> Z { get; }

        protected IAxisInput XInput { get; private set; }

        protected IAxisInput YInput { get; private set; }

        protected IAxisInput ZInput { get; private set; }

        public override bool Valid
        {
            get { return base.Valid && XInput != null && YInput != null && ZInput != null; }
        }

        protected Vector3Control(IInputManager inputManager) : base(inputManager)
        {
        }

        protected override ICollection<IBindingKey> PrepareBindings()
        {
            return new List<IBindingKey> {X, Y, Z};
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

            if (Equals(key, Z))
            {
                ZInput = InputManager.Get(Z);
            }
        }

        protected override IObservable<Vector3> Observe()
        {
            return Observable
                .Zip(XInput.Value, YInput.Value, ZInput.Value)
                .Select(xs => new Vector3(xs[0], xs[1], xs[2]));
        }
    }
}