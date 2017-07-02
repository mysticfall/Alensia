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
    }
}