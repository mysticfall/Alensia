using Alensia.Core.Common;

namespace Alensia.Core.UI
{
    public interface IUIHandler : IComponentHandler, ICloseableOnce
    {
    }

    namespace Generic
    {
        public interface IUIHandler<out T> : IUIHandler, IComponentHandler<T> 
            where T : IComponent
        {
        }
    }
}