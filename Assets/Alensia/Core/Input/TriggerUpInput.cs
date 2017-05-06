using System.Collections.Generic;
using System.Linq;
using UniRx;

namespace Alensia.Core.Input
{
    public class TriggerUpInput : TriggerInput
    {
        public TriggerUpInput(ITrigger trigger) : base(trigger)
        {
        }

        public TriggerUpInput(ITrigger trigger, ITrigger modifier) : base(trigger, modifier)
        {
        }

        public TriggerUpInput(IList<ITrigger> triggers) : base(triggers)
        {
        }

        public TriggerUpInput(ITrigger trigger, IList<ITrigger> modifiers) :
            base(trigger, modifiers)
        {
        }

        public TriggerUpInput(IList<ITrigger> triggers, IList<ITrigger> modifiers) :
            base(triggers, modifiers)
        {
        }

        protected override IObservable<float> Observe(IObservable<long> onTick)
        {
            return onTick.Where(_ => Triggers.All(t => t.Up)).Select(_ => 1f);
        }
    }
}