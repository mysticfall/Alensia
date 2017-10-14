using System.Collections.Generic;
using System.Linq;
using Alensia.Core.Common;
using Alensia.Core.UI;
using Alensia.Core.UI.Cursor;
using UniRx;
using UnityEngine;
using Zenject;

namespace Alensia.Core.Control
{
    public class Controller : ActivatableMonoBehavior, IController
    {
        [Inject]
        public IRuntimeUIContext UIContext { get; }

        public CursorState DefaultCursorState => UIContext.CursorState;

        public IEnumerable<IControl> Controls => _controls.ToList();

        [Inject] private IList<IControl> _controls;

        protected override void OnInitialized()
        {
            DefaultCursorState.Apply();

            base.OnInitialized();

            Activate();

            Controls
                .Select(c => c.OnActiveStateChange)
                .Merge()
                .Subscribe(_ => CheckInputStatus(), Debug.LogError)
                .AddTo(this);

            CheckInputStatus();
        }

        protected override void OnDisposed()
        {
            Deactivate();

            base.OnDisposed();
        }

        private void CheckInputStatus()
        {
            var control = Controls.FirstOrDefault(c => c.Active && c.CursorState != null);
            var state = control?.CursorState ?? DefaultCursorState;

            state.Apply();
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