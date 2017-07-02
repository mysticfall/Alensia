using System;
using Alensia.Core.Control;
using Alensia.Core.UI;
using UniRx;
using UnityEngine;
using Zenject;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace Alensia.Demo
{
    public class MainMenuHandler : UIHandler
    {
        [Inject, NonSerialized] public IPlayerController Controller;

        public Button ButtonResume;

        public Button ButtonQuit;

        public override void Initialize(IUIContext context)
        {
            base.Initialize(context);

            ButtonResume.OnClick.Subscribe(_ => Close()).AddTo(this);
            ButtonQuit.OnClick.Subscribe(_ => Quit()).AddTo(this);

            OnClose.Subscribe(_ => EnableControls());

            DisableControls();
        }

        protected virtual void DisableControls()
        {
            Controller.DisablePlayerControl();

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        protected virtual void EnableControls()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            Controller.EnablePlayerControl();
        }

        protected virtual void Quit()
        {
            Close();

#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        public class Factory : Factory<MainMenuHandler>
        {
        }
    }
}