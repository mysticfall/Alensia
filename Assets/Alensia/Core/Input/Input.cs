using Alensia.Core.Input.Generic;
using UniRx;

namespace Alensia.Core.Input
{
    public abstract class Input<T> : IInput<T>
    {
        public IReadOnlyReactiveProperty<T> Value => _value;

        private ReadOnlyReactiveProperty<T> _value;

        private Subject<long> _subject;

        private long _count;

        protected virtual IObservable<long> ObserveTick()
        {
            return _subject;
        }

        protected abstract IObservable<T> Observe(IObservable<long> onTick);

        public virtual void Initialize()
        {
            //TODO Can't rely on Observable.EveryUpdate() since it's invoked later than every other ITickables.
            _subject = new Subject<long>();
            _count = 0;

            var onTick = ObserveTick();

            _value = new ReadOnlyReactiveProperty<T>(Observe(onTick), false);
        }

        public virtual void Dispose()
        {
            _subject = null;
            _count = 0;

            if (_value == null) return;

            _value.Dispose();
            _value = null;
        }

        public void Tick()
        {
            _subject?.OnNext(_count++);
        }
    }
}