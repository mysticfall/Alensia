using Alensia.Core.UI.Generic;

namespace Alensia.Core.UI
{
    public abstract class ComponentHandler<T> : UIElement, IComponentHandler<T>
        where T : class, IComponent
    {
        public T Component => _component ?? (_component = GetComponent<T>());

        IComponent IComponentHandler.Component => Component;

        private T _component;

        public override void Initialize(IUIContext context)
        {
            base.Initialize(context);

            if (Component.Context == null)
            {
                Component.Initialize(context);
            }
        }
    }
}