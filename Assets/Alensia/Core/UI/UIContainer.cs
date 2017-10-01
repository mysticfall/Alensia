using System.Collections.Generic;
using System.Linq;
using Alensia.Core.Common;
using UnityEngine;

namespace Alensia.Core.UI
{
    public abstract class UIContainer : UIComponent, IContainer
    {
        public virtual IList<IComponent> Children => transform.GetChildren<IComponent>().ToList();

        public override void Initialize(IUIContext context)
        {
            base.Initialize(context);

            if (!Application.isPlaying) return;

            foreach (var child in Children)
            {
                child.Initialize(Context);
            }
        }
    }
}