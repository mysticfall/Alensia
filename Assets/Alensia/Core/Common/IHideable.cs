using UniRx;

namespace Alensia.Core.Common
{
    public interface IHideable
    {
        IReactiveProperty<bool> Visible { get; }

        void Show();

        void Hide();

        IObservable<Unit> OnShow { get; }

        IObservable<Unit> OnHide { get; }
    }
}