using System;
using System.Collections.Generic;
using System.Linq;
using Alensia.Core.Common;
using Alensia.Core.UI.Cursor;
using UniRx;
using UnityEngine.Assertions;

namespace Alensia.Core.Control
{
    public class Controller : BaseActivatable, IController
    {
        public CursorState DefaultCursorState => _settings.DefaultCursorState;

        public IReadOnlyList<IControl> Controls { get; }

        private readonly Settings _settings;

        public Controller(List<IControl> controls) : this(null, controls)
        {
        }

        //TODO Change to IList later: https://github.com/modesttree/Zenject/issues/281
        public Controller(Settings settings, List<IControl> controls)
        {
            Assert.IsNotNull(controls, "controls != null");
            Assert.IsTrue(controls.Any(), "controls.Any()");

            Controls = controls.AsReadOnly();

            _settings = settings ?? new Settings();
        }

        protected override void OnInitialized()
        {
            DefaultCursorState.Apply();

            base.OnInitialized();

            Activate();

            Controls
                .Select(c => c.OnActiveStateChange)
                .Merge()
                .Subscribe(_ => CheckInputStatus())
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

        [Serializable]
        public class Settings : IEditorSettings
        {
            public CursorState DefaultCursorState = CursorState.Vislbe;
        }
    }
}