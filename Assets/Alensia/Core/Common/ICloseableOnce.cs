using UniRx;

namespace Alensia.Core.Common
{
    public interface ICloseableOnce
    {
        IReadOnlyReactiveProperty<bool> Closed { get; }

        IObservable<Unit> OnClose { get; }

        void Close();
    }
}