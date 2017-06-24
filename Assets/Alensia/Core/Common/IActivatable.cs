using UniRx;

namespace Alensia.Core.Common
{
    public interface IActivatable
    {
        IReactiveProperty<bool> Active { get; }

        void Activate();

        void Deactivate();

        IObservable<Unit> OnActivate { get; }

        IObservable<Unit> OnDeactivate { get; }
    }
}