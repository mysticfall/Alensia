using Alensia.Core.Common;
using Alensia.Core.Input.Generic;
using UniRx;

namespace Alensia.Core.Input
{
    public abstract class Input<T> : BaseObject, IInput<T>
    {
        public T Value => _value.Value;

        public IObservable<T> OnChange => _observable.AsObservable();

        protected virtual IObservable<long> OnTick => _tick;

        protected abstract IObservable<T> Observe(IObservable<long> onTick);

        private IObservable<T> _observable;

        private IReadOnlyReactiveProperty<T> _value;

        private readonly Subject<long> _tick;

        private long _count;

        protected Input()
        {
            _tick = new Subject<long>();

            OnInitialize.Subscribe(_ => AfterInitialize()).AddTo(this);
        }

        private void AfterInitialize()
        {
            _observable = Observe(OnTick);
            _value = _observable.ToReadOnlyReactiveProperty();
        }

        //TODO Can't rely on Observable.EveryUpdate() since it's invoked later than every other ITickables.
        public void Tick() => _tick.OnNext(_count++);
    }
}