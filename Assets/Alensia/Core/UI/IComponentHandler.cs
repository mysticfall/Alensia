namespace Alensia.Core.UI
{
    public interface IComponentHandler : IUIElement
    {
        IComponent Component { get; }
    }

    namespace Generic
    {
        public interface IComponentHandler<out T> : IComponentHandler where T : IComponent
        {
            new T Component { get; }
        }
    }
}