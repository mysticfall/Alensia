using Alensia.Core.UI;
using UniRx;
using UnityEditor;
using UnityEngine;

namespace Alensia.Demo.Controller
{
    public class MainMenu : Box
    {
        protected readonly CompositeDisposable Observers;

        public MainMenu(IUIManager manager) : base(
            new BoxLayout(BoxLayout.BoxOrientation.Vertical), manager)
        {
            Observers = new CompositeDisposable();
        }

        protected override void Initialize()
        {
            base.Initialize();

            Text = "Main Menu";

            var btnQuit = new Button(Manager)
            {
                Text = "Quit to Desktop",
                Padding = new RectOffset(30, 30, 10, 10),
                Color = UnityEngine.Color.red
            };

            var btnDismiss = new Button(Manager)
            {
                Text = "Return to Game",
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

            if (EditorApplication.isPlaying)
            {
                EditorApplication.isPlaying = false;
            }
            else
            {
                Application.Quit();
            }
        }

        protected virtual void OnDismiss() => Dispose();

        private void Dispose()
        {
            Manager.Remove(this);

            Observers.Dispose();
        }
    }
}