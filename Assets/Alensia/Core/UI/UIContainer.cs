using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Alensia.Core.UI
{
    public abstract class UIContainer : UIComponent, IContainer
    {
        public IList<IComponent> Children => transform.Cast<Transform>()
            .Select(c => c.GetComponent<IComponent>())
            .Where(c => c != null)
            .ToList();

        public override void Initialize(IUIContext context)
        {
            base.Initialize(context);

            foreach (var child in Children)
            {
                child.Initialize(Context);
            }
        }
    }
}