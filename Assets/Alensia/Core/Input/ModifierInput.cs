using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine.Assertions;

namespace Alensia.Core.Input
{
    public abstract class ModifierInput<T> : Input<T>, IModifierInput
    {
        public IList<ITrigger> Modifiers { get; }

        protected override IObservable<long> OnTick => 
            base.OnTick.Where(_ => Modifiers.All(t => t.Hold));

        protected ModifierInput() :
            this(Enumerable.Empty<ITrigger>().ToList())
        {
        }

        protected ModifierInput(IList<ITrigger> modifiers)
        {
            Assert.IsNotNull(modifiers, "modifiers != null");

            Modifiers = modifiers.ToList().AsReadOnly();
        }
    }
}