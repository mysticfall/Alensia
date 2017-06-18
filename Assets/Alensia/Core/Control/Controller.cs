using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Assertions;
using Zenject;

namespace Alensia.Core.Control
{
    public class Controller : IController, IInitializable, IDisposable
    {
        public IReadOnlyList<IControl> Controls { get; }

        public bool Active
        {
            get { return _active; }
            set
            {
                lock (this)
                {
                    if (_active == value) return;

                    foreach (var control in Controls)
                    {
                        control.Active = value;
                    }

                    _active = value;
                }
            }
        }

        private readonly IDictionary<string, IControl> _controls;

        private bool _active;

        //TODO We can't use IList here due to a Zenject's limitation.
        public Controller(List<IControl> controls)
        {
            Assert.IsNotNull(controls, "controls != null");
            Assert.IsTrue(controls.Any(), "controls.Any()");

            Controls = controls.AsReadOnly();

            _controls = new Dictionary<string, IControl>();

            foreach (var control in Controls)
            {
                _controls.Add(control.Name, control);
            }
        }

        public virtual void Initialize() => Active = true;

        public virtual void Dispose() => Active = false;

        public bool Contains(string key) => _controls.ContainsKey(key);

        public IControl this[string key] => _controls.ContainsKey(key) ? _controls[key] : null;

        public virtual void EnableControl(string name)
        {
            var control = this[name];

            if (control != null && !control.Active) control.Active = true;
        }

        public virtual void DisableControl(string name)
        {
            var control = this[name];

            if (control != null && control.Active) control.Active = false;
        }
    }
}