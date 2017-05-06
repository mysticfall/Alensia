using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine.Assertions;

namespace Alensia.Core.Input
{
    public class AxisInput : ModifierInput<float>, IAxisInput
    {
        public string Axis { get; private set; }

        public AxisInput(string axis) : this(axis, Enumerable.Empty<ITrigger>().ToList())
        {
        }

        public AxisInput(string axis, IList<ITrigger> modifiers) : base(modifiers)
        {
            Assert.IsNotNull(axis, "axis != null");

            Axis = axis;
        }

        protected override IObservable<float> Observe(IObservable<long> onTick)
        {
            return onTick.Select(_ => UnityEngine.Input.GetAxis(Axis));
        }
    }
}