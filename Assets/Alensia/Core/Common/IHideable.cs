using UniRx;

namespace Alensia.Core.Common
{
    public interface IHideable
    {
        bool Visible { get; set; }

        void Show();

        void Hide();

        IObservable<Unit> OnShow { get; }

        IObservable<Unit> OnHide { get; }

        IObservable<bool> OnVisibilityChange { get; }
    }
}