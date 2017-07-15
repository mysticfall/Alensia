using UniRx;

namespace Alensia.Core.Common
{
    public abstract class BaseActivatable : BaseObject, IActivatable
    {
        public bool Active
        {
            get { return _active.Value; }
            set
            {
                if (Initialized)
                {
                    _active.Value = value;
                }
                else
                {
                    _lazyActivation = value;
                }
            }
        }

        public IObservable<Unit> OnActivate => _active.Where(v => v).AsUnitObservable();

        public IObservable<Unit> OnDeactivate => _active.Where(v => !v).AsUnitObservable();

        public IObservable<bool> OnActiveStateChange => _active;

        private bool _lazyActivation;

        private readonly IReactiveProperty<bool> _active;

        protected BaseActivatable()
        {
            _active = new ReactiveProperty<bool>();
        }

        public override void Initialize()
        {
            base.Initialize();

            if (_lazyActivation) Activate();
        }

        public virtual void Activate() => Active = true;

        public virtual void Deactivate() => Active = false;
    }
}