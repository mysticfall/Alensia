using System.Collections.Generic;
using System.Linq;
using Alensia.Core.Common;
using UnityEngine;
using UnityEngine.Assertions;

namespace Alensia.Core.Control
{
    public class Controller : BaseActivatable, IController
    {
        public IReadOnlyList<IControl> Controls { get; }

        //TODO Change to IList later: https://github.com/modesttree/Zenject/issues/281
        public Controller(List<IControl> controls)
        {
            Assert.IsNotNull(controls, "controls != null");
            Assert.IsTrue(controls.Any(), "controls.Any()");

            Controls = controls.AsReadOnly();

            //TODO Find a better way to handle cursor mode & visibility.
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            Activate();
        } 

        protected override void OnDisposed()
        {
            Deactivate();

            base.OnDisposed();
        }

        public T FindControl<T>() where T : IControl => FindControls<T>().FirstOrDefault();

        public IReadOnlyList<T> FindControls<T>() where T : IControl => Controls.OfType<T>().ToList();

        public void EnableControls<T>() where T : IControl
        {
            foreach (var control in FindControls<T>())
            {
                control.Activate();
            }
        }

        public void DisableControls<T>() where T : IControl
        {
            foreach (var control in FindControls<T>())
            {
                control.Deactivate();
            }
        }
    }
}