using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;

namespace Alensia.Core.Input
{
    public class TriggerTabInput : TriggerInput
    {
        public TriggerTabInput(ITrigger trigger) : base(trigger)
        {
        }

        public TriggerTabInput(ITrigger trigger, ITrigger modifier) : base(trigger, modifier)
        {
        }

        public TriggerTabInput(IList<ITrigger> triggers) : base(triggers)
        {
        }

        public TriggerTabInput(ITrigger trigger, IList<ITrigger> modifiers) :
            base(trigger, modifiers)
        {
        }

        public TriggerTabInput(IList<ITrigger> triggers, IList<ITrigger> modifiers) :
            base(triggers, modifiers)
        {
        }

        protected override UniRx.IObservable<float> Observe(UniRx.IObservable<long> onTick)
        {
            var source = onTick
                .Where(_ => Triggers.All(t => t.Up))
                .Throttle(TimeSpan.FromSeconds(0.25));

            return source
                .Buffer(source)
                .Where(xs => xs.Count > 1)
                .Select(xs => (float) xs.Count);
        }
    }
}