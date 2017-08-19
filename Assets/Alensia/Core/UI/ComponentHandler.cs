using Alensia.Core.UI.Generic;

namespace Alensia.Core.UI
{
    public abstract class ComponentHandler<T> : UIElement, IComponentHandler<T>
        where T : IComponent
    {
        public T Component { get; private set; }

        IComponent IComponentHandler.Component => Component;

        public override void Initialize(IUIContext context)
        {
            base.Initialize(context);

            if (Component.Context == null)
            {
                Component.Initialize(context);
            }
        }

        protected override void InitializePeers()
        {
            Component = GetComponent<T>();

            base.InitializePeers();
        }
    }
}