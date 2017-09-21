using System.Linq;
using Alensia.Core.Control;
using Alensia.Core.Game;
using Alensia.Core.I18n;
using Alensia.Core.UI;
using UniRx;
using Zenject;

namespace Alensia.Demo
{
    public class MainMenuHandler : UIHandler<Panel>
    {
        [Inject]
        public IGame Game { get; }

        [Inject]
        public IPlayerController Controller { get; }

        [Inject]
        public IUIManager UIManager { get; }

        [Inject]
        public ILocaleService LocaleService { get; }

        public Dropdown ChoiceLanguage;

        public Dropdown ChoiceStyle;

        public Button ButtonResume;

        public Button ButtonQuit;

        public UIStyle[] Styles;

        private bool _playerControlEnabled;

        public override void Initialize(IUIContext context)
        {
            base.Initialize(context);

            ButtonResume.OnPointerSelect
                .Subscribe(_ => Close())
                .AddTo(this);

            ButtonQuit.OnPointerSelect
                .Subscribe(_ => Game.Quit())
                .AddTo(this);

            OnClose
                .Where(_ => Controller.Active)
                .Subscribe(_ => ResumeGame(_playerControlEnabled));

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

            ChoiceStyle.Items = Styles.Select(s => new DropdownItem(s.Name, s.Name)).ToList();
            ChoiceStyle.Value = UIManager.Style.Name;
            ChoiceStyle.OnValueChange
                .Select(v => Styles.ToList().Find(s => s.Name == v))
                .Subscribe(s => UIManager.Style = s)
                .AddTo(this);

            _playerControlEnabled = Controller.PlayerControlEnabled;

            PauseGame();
        }

        protected virtual void PauseGame()
        {
            Game.Pause();

            Controller.DisablePlayerControl();
        }

        protected virtual void ResumeGame(bool enableControls)
        {
            if (enableControls)
            {
                Controller.EnablePlayerControl();
            }

            Game.Resume();
        }
    }
}