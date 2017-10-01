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
            if (Application.isPlaying)
            {
                InitializeChildren(context);
            }

            base.Initialize(context);
        }

        protected virtual void InitializeChildren(IUIContext context)
        {
            foreach (var child in Children)
            {
                child.Initialize(context);
            }
        }
    }
}