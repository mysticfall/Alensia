using System.Collections.Generic;
using System.Linq;
using Alensia.Core.Common;
using Alensia.Core.Input.Generic;
using UniRx;
using Zenject;

namespace Alensia.Core.Input
{
    public class InputManager : BaseObject, IInputManager, ITickable
    {
        public ICollection<IBindingKey> Keys => _bindings.AsReadOnly();

        public IObservable<IBindingKey> OnBindingChange => _onBindingChange;

        private readonly List<IBindingKey> _bindings;

        private readonly IDictionary<IBindingKey, IInput> _bindingMap;

        private readonly Subject<IBindingKey> _onBindingChange;

        public InputManager()
        {
            _bindings = new List<IBindingKey>();
            _bindingMap = new Dictionary<IBindingKey, IInput>();

            OnDispose.Subscribe(_ => AfterDispose()).AddTo(this);

            _onBindingChange = new Subject<IBindingKey>();
        }

        private void AfterDispose()
        {
            lock (this)
            {
                var keys = new List<IBindingKey>(Keys.Reverse());

                foreach (var key in keys)
                {
                    Deregister(key);
                }
            }
        }

        public bool Contains(IBindingKey key)
        {
            lock (this)
            {
                return _bindingMap.ContainsKey(key);
            }
        }

        public T Get<T>(IBindingKey<T> key) where T : class, IInput
        {
            lock (this)
            {
                IInput binding;

                _bindingMap.TryGetValue(key, out binding);

                return binding as T;
            }
        }

        public void Register<T>(IBindingKey<T> key, T input) where T : class, IInput
        {
            lock (this)
            {
                Deregister(key);

                _bindings.Add(key);
                _bindingMap.Add(key, input);
            }

            input.Initialize();

            _onBindingChange.OnNext(key);
        }

        public void Deregister(IBindingKey key)
        {
            lock (this)
            {
                if (!_bindingMap.ContainsKey(key)) return;

                var existingInput = _bindingMap[key];

                existingInput.Dispose();

                _bindings.Remove(key);
                _bindingMap.Remove(key);
            }

            _onBindingChange.OnNext(key);
        }

        public void Tick()
        {
            // ReSharper disable once InconsistentlySynchronizedField
            foreach (var input in _bindingMap.Values)
            {
                input.Tick();
            }
        }
    }
}