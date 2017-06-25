using UniRx;

namespace Alensia.Core.Common
{
    public abstract class BaseActivatable : BaseObject, IActivatable
    {
        public bool Active
        {
            get { return _active.Value; }
            set { _active.Value = value; }
        }

        public IObservable<Unit> OnActivate => _active.Where(v => v).Select(_ => Unit.Default);

        public IObservable<Unit> OnDeactivate => _active.Where(v => !v).Select(_ => Unit.Default);

        public IObservable<bool> OnActiveStateChange => _active;

        private readonly IReactiveProperty<bool> _active;

        protected BaseActivatable()
        {
            _active = new ReactiveProperty<bool>();
        }

        public void Activate() => _active.Value = true;

        public void Deactivate() => _active.Value = false;
    }
}