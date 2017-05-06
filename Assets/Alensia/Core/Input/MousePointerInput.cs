using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Alensia.Core.Input
{
    public class MousePointerInput : ModifierInput<Vector3>, IPositionInput
    {
        public MousePointerInput()
        {
        }

        public MousePointerInput(IList<ITrigger> modifiers) : base(modifiers)
        {
        }

        protected override IObservable<Vector3> Observe(IObservable<long> onTick)
        {
            return onTick.Select(_ => UnityEngine.Input.mousePosition);
        }
    }
}