using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using static UnityEngine.Input;

namespace Alensia.Core.Input
{
    public class AxisInput : ModifierInput<float>, IAxisInput
    {
        public string Axis { get; }

        public float? Smoothing { get; }

        public AxisInput(string axis) :
            this(axis, null, Enumerable.Empty<ITrigger>().ToList())
        {
        }

        public AxisInput(string axis, float? smoothing) :
            this(axis, smoothing, Enumerable.Empty<ITrigger>().ToList())
        {
        }

        public AxisInput(string axis, float? smoothing, IList<ITrigger> modifiers) : base(modifiers)
        {
            Assert.IsNotNull(axis, "axis != null");

            Axis = axis;
            Smoothing = smoothing;
        }

        protected override IObservable<float> Observe(IObservable<long> onTick)
        {
            if (Smoothing.HasValue)
            {
                return onTick
                    .Select(_ => GetAxis(Axis))
                    .Scan((previous, current) =>
                        Mathf.Lerp(previous, current, Time.deltaTime / Smoothing.Value));
            }

            return onTick.Select(_ => GetAxis(Axis));
        }
    }
}