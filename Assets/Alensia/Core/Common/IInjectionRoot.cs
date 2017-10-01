using Zenject;

namespace Alensia.Core.Common
{
    public interface IInjectionRoot
    {
        DiContainer DiContainer { get; }
    }
}