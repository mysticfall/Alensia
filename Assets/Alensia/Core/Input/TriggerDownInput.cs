using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;

namespace Alensia.Core.Input
{
    public class TriggerDownInput : TriggerInput
    {
        public TriggerDownInput(ITrigger trigger) : base(trigger)
        {
        }

        public TriggerDownInput(ITrigger trigger, ITrigger modifier) : base(trigger, modifier)
        {
        }

        public TriggerDownInput(IList<ITrigger> triggers) : base(triggers)
        {
        }

        public TriggerDownInput(ITrigger trigger, IList<ITrigger> modifiers) :
            base(trigger, modifiers)
        {
        }

        public TriggerDownInput(IList<ITrigger> triggers, IList<ITrigger> modifiers) :
            base(triggers, modifiers)
        {
        }

        protected override IObservable<float> Observe(IObservable<long> onTick)
        {
            return onTick.Where(_ => Triggers.All(t => t.Down)).Select(_ => 1f);
        }
    }
}