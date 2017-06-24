using Alensia.Core.Common;
using Alensia.Core.Input.Generic;
using UniRx;

namespace Alensia.Core.Input
{
    public abstract class Input<T> : BaseObject, IInput<T>
    {
        public IObservable<T> Value { get; private set; }

        protected virtual IObservable<long> OnTick => _count;

        protected abstract IObservable<T> Observe(IObservable<long> onTick);

        private readonly ReactiveProperty<long> _count;

        protected Input()
        {
            _count = new ReactiveProperty<long>();

            OnInitialize.Subscribe(_ => Value = Observe(OnTick)).AddTo(this);
            OnDispose.Subscribe(_ => _count.Dispose()).AddTo(this);
        }

        //TODO Can't rely on Observable.EveryUpdate() since it's invoked later than every other ITickables.
        public void Tick() => _count.Value += 1;
    }
}