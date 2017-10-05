using System;
using Alensia.Core.Common;
using UniRx;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace Alensia.Core.Game
{
    public class Game : ManagedMonoBehavior, IGame
    {
        public float TimeScale
        {
            get { return _timeScale.Value; }
            set { _timeScale.Value = value; }
        }

        public bool Paused => _paused.Value;

        public IObservable<float> OnTimeScaleChange => _timeScale;

        public IObservable<bool> OnPauseStateChange => _paused;

        public IObservable<Unit> OnPause => OnPauseStateChange.Where(s => s).AsSingleUnitObservable();

        public IObservable<Unit> OnResume => OnPauseStateChange.Where(s => !s).AsSingleUnitObservable();

        [SerializeField] private FloatReactiveProperty _timeScale;

        [SerializeField] private BoolReactiveProperty _paused;

        public Game()
        {
            _timeScale = new FloatReactiveProperty(1);
            _paused = new BoolReactiveProperty();
        }

        protected override void OnInitialized()
        {
            _timeScale
                .Where(_ => !Paused)
                .Subscribe(scale => Time.timeScale = scale, Debug.LogError)
                .AddTo(this);
            _paused
                .Select(v => v ? 0 : TimeScale)
                .Subscribe(scale => Time.timeScale = scale, Debug.LogError)
                .AddTo(this);

            base.OnInitialized();
        }

        public void Pause() => _paused.Value = true;

        public void Resume() => _paused.Value = false;

        public void Quit()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}