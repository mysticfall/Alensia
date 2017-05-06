using System;
using System.Collections.Generic;
using System.Linq;
using Alensia.Core.Input.Generic;
using UnityEngine.Assertions;
using Zenject;

namespace Alensia.Core.Input
{
    public class InputManager : IInputManager, ITickable, IDisposable
    {
        public ICollection<IBindingKey> Keys
        {
            get { return _bindings.AsReadOnly(); }
        }

        public BindingChangeEvent BindingChanged { get; private set; }

        private readonly List<IBindingKey> _bindings;

        private readonly IDictionary<IBindingKey, IInput> _bindingMap;

        private bool _disposed;

        public InputManager(BindingChangeEvent bindingChanged)
        {
            Assert.IsNotNull(bindingChanged, "bindingChanged != null");

            BindingChanged = bindingChanged;

            _bindings = new List<IBindingKey>();
            _bindingMap = new Dictionary<IBindingKey, IInput>();
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
                return _bindingMap[key] as T;
            }
        }

        public void Register<T>(IBindingKey<T> key, T input) where T : class, IInput
        {
            lock (this)
            {
                CheckStatus();

                Deregister(key);

                _bindings.Add(key);
                _bindingMap.Add(key, input);
            }

            input.Initialize();

            BindingChanged.Fire(key);
        }

        public void Deregister(IBindingKey key)
        {
            lock (this)
            {
                CheckStatus();

                if (!_bindingMap.ContainsKey(key)) return;

                var existingInput = _bindingMap[key];

                existingInput.Dispose();

                _bindings.Remove(key);
                _bindingMap.Remove(key);
            }

            BindingChanged.Fire(key);
        }

        public void Dispose()
        {
            lock (this)
            {
                CheckStatus();

                var keys = new List<IBindingKey>(Keys.Reverse());

                foreach (var key in keys)
                {
                    Deregister(key);
                }

                _disposed = true;
            }
        }

        public void Tick()
        {
            // ReSharper disable once InconsistentlySynchronizedField
            foreach (var input in _bindingMap.Values)
            {
                input.Tick();
            }
        }

        private void CheckStatus()
        {
            if (_disposed)
            {
                throw new InvalidOperationException(
                    "The component was disposed already.");
            }
        }
    }
}