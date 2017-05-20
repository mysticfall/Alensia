using System.Collections.Generic;
using System.Linq;
using UniRx;

namespace Alensia.Core.Input
{
    public class TriggerStateInput : TriggerInput
    {
        public TriggerStateInput(ITrigger trigger) : base(trigger)
        {
        }

        public TriggerStateInput(ITrigger trigger, ITrigger modifier) : base(trigger, modifier)
        {
        }

        public TriggerStateInput(IList<ITrigger> triggers) : base(triggers)
        {
        }

        public TriggerStateInput(ITrigger trigger, IList<ITrigger> modifiers) :
            base(trigger, modifiers)
        {
        }

        public TriggerStateInput(IList<ITrigger> triggers, IList<ITrigger> modifiers) :
            base(triggers, modifiers)
        {
        }

        protected override IObservable<float> Observe(IObservable<long> onTick)
        {
            return onTick
                .Select(_ => Triggers.All(t => t.Hold))
                .Select(state => state ? 1f : 0f);
        }
    }
}