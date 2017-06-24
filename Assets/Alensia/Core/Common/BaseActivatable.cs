using UniRx;

namespace Alensia.Core.Common
{
    public abstract class BaseActivatable : BaseObject, IActivatable
    {
        public IReactiveProperty<bool> Active { get; }

        public IObservable<Unit> OnActivate => Active.Where(v => v).Select(_ => Unit.Default);

        public IObservable<Unit> OnDeactivate => Active.Where(v => !v).Select(_ => Unit.Default);

        protected BaseActivatable()
        {
            Active = new ReactiveProperty<bool>();
        }

        public void Activate() => Active.Value = true;

        public void Deactivate() => Active.Value = false;
    }
}