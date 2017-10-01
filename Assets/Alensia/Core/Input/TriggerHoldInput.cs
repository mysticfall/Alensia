using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;

namespace Alensia.Core.Input
{
    public class TriggerHoldInput : TriggerInput
    {
        public TriggerHoldInput(ITrigger trigger) : base(trigger)
        {
        }

        public TriggerHoldInput(ITrigger trigger, ITrigger modifier) : base(trigger, modifier)
        {
        }

        public TriggerHoldInput(IList<ITrigger> triggers) : base(triggers)
        {
        }

        public TriggerHoldInput(ITrigger trigger, IList<ITrigger> modifiers) :
            base(trigger, modifiers)
        {
        }

        public TriggerHoldInput(IList<ITrigger> triggers, IList<ITrigger> modifiers) :
            base(triggers, modifiers)
        {
        }

        protected override IObservable<float> Observe(IObservable<long> onTick)
        {
            return onTick.Where(_ => Triggers.All(t => t.Hold)).Select(_ => 1f);
        }
    }
}