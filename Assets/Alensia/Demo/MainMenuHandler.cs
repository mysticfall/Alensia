using System;
using System.Linq;
using Alensia.Core.Control;
using Alensia.Core.Game;
using Alensia.Core.I18n;
using Alensia.Core.UI;
using UniRx;
using UnityEngine;
using Zenject;

#if UNITY_EDITOR

#endif

namespace Alensia.Demo
{
    public class MainMenuHandler : UIHandler
    {
        [Inject, NonSerialized] public IGame Game;

        [Inject, NonSerialized] public IPlayerController Controller;

        [Inject, NonSerialized] public ILocaleService LocaleService;

        public Dropdown ChoiceLanguage;

        public Button ButtonResume;

        public Button ButtonQuit;

        public override void Initialize(IUIContext context)
        {
            base.Initialize(context);

            ButtonResume.OnClick.Subscribe(_ => Close()).AddTo(this);
            ButtonQuit.OnClick.Subscribe(_ => Game.Quit()).AddTo(this);

            OnClose.Where(_ => Controller.Active).Subscribe(_ => EnableControls());

            LocaleService.OnLocaleChange
                .Select(_ => LocaleService.SupportedLocales)
                .Select(l => l.Select(i => new DropdownItem(i.ToString(), i.NativeName)))
                .Subscribe(i => ChoiceLanguage.Items = i.ToList())
                .AddTo(this);

            ChoiceLanguage.Value = LocaleService.CurrentLocale.ToString();
            ChoiceLanguage.OnValueChange
                .Select(k => new LanguageTag(k).ToCulture())
                .Subscribe(l => LocaleService.CurrentLocale = l)
                .AddTo(this);

            DisableControls();
        }

        protected virtual void DisableControls()
        {
            Game.Pause();

            Controller.DisablePlayerControl();

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        protected virtual void EnableControls()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            Controller.EnablePlayerControl();

            Game.Resume();
        }
    }
}