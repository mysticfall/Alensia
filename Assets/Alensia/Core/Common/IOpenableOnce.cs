using UniRx;

namespace Alensia.Core.Common
{
    public interface IOpenableOnce
    {
        IReadOnlyReactiveProperty<bool> Opened { get; }

        IObservable<Unit> OnOpen { get; }

        void Open();
    }
}