using Alensia.Core.I18n;
using Alensia.Core.UI;
using Alensia.Core.UI.Legacy;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace Alensia.Demo
{
    public class MainMenu : Box
    {
        protected readonly ITranslator Translator;

        protected readonly CompositeDisposable Observers;

        public MainMenu(ITranslator translator, IUIManager manager) : base(
            new BoxLayout(BoxLayout.BoxOrientation.Vertical), manager)
        {
            Assert.IsNotNull(translator, "translator != null");

            Observers = new CompositeDisposable();
            Translator = translator;
        }

        protected override void Initialize()
        {
            base.Initialize();

            Text = Translator["windows.MainMenu.title"];

            var btnQuit = new Button(Manager)
            {
                Text = Translator["windows.MainMenu.buttons.quit"],
                Padding = new RectOffset(30, 30, 10, 10),
                Color = UnityEngine.Color.red
            };

            var btnDismiss = new Button(Manager)
            {
                Text = Translator["windows.MainMenu.buttons.resume"],
                Padding = new RectOffset(30, 30, 10, 10)
            };

            Add(btnQuit);
            Add(btnDismiss);

            this.Pack();
            this.CenterOnScreen();

            btnQuit.Clicked
                .Subscribe(_ => OnQuit())
                .AddTo(Observers);

            btnDismiss.Clicked
                .Subscribe(_ => OnDismiss())
                .AddTo(Observers);
        }

        protected virtual void OnQuit()
        {
            Dispose();

#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        protected virtual void OnDismiss() => Dispose();

        private void Dispose()
        {
            Manager.Remove(this);

            Observers.Dispose();
        }
    }
}