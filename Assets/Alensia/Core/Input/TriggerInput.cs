using System.Collections.Generic;
using System.Linq;
using UnityEngine.Assertions;

namespace Alensia.Core.Input
{
    public abstract class TriggerInput : ModifierInput<float>, ITriggerInput
    {
        public IList<ITrigger> Triggers { get; private set; }

        protected TriggerInput(ITrigger trigger) :
            this(new List<ITrigger> {trigger}, Enumerable.Empty<ITrigger>().ToList())
        {
        }

        protected TriggerInput(ITrigger trigger, ITrigger modifier) :
            this(new List<ITrigger> {trigger}, new List<ITrigger> {modifier})
        {
        }

        protected TriggerInput(IList<ITrigger> triggers) :
            this(triggers, Enumerable.Empty<ITrigger>().ToList())
        {
        }

        protected TriggerInput(ITrigger trigger, IList<ITrigger> modifiers) :
            this(new List<ITrigger> {trigger}, modifiers)
        {
        }

        protected TriggerInput(IList<ITrigger> triggers, IList<ITrigger> modifiers) :
            base(modifiers)
        {
            Assert.IsNotNull(triggers, "triggers != null");
            Assert.IsTrue(triggers.Any(), "triggers.Any()");

            Triggers = triggers.ToList().AsReadOnly();
        }
    }
}