using System.Collections.Generic;
using Alensia.Core.Input;
using Alensia.Core.Input.Generic;
using UniRx;

namespace Alensia.Core.Control
{
    public abstract class ScalarControl : Control<float>
    {
        public abstract IBindingKey<IAxisInput> Axis { get; }

        protected IAxisInput AxisInput { get; private set; }

        public override bool Valid
        {
            get { return base.Valid && AxisInput != null; }
        }

        protected ScalarControl(IInputManager inputManager) : base(inputManager)
        {
        }

        protected override ICollection<IBindingKey> PrepareBindings()
        {
            return new List<IBindingKey> {Axis};
        }

        protected override void OnBindingChange(IBindingKey key)
        {
            base.OnBindingChange(key);

            if (Equals(key, Axis))
            {
                AxisInput = InputManager.Get(Axis);
            }
        }

        protected override IObservable<float> Observe()
        {
            return AxisInput.Value;
        }
    }
}