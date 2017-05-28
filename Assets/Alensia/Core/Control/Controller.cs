using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Assertions;
using Zenject;

namespace Alensia.Core.Control
{
    public class Controller : IController, IInitializable, IDisposable
    {
        public IList<IControl> Controls { get; }

        public Controller(List<IControl> controls)
        {
            Assert.IsNotNull(controls, "controls != null");
            Assert.IsTrue(controls.Any(), "controls.Any()");

            Controls = controls.AsReadOnly();
        }

        public virtual void Initialize()
        {
            foreach (var control in Controls)
            {
                control.Active = true;
            }
        }

        public virtual void Dispose()
        {
            foreach (var control in Controls)
            {
                control.Active = false;
            }
        }
    }
}