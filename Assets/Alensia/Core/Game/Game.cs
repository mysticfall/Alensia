using System;
using Alensia.Core.Common;
using UniRx;
using UnityEngine;
using Zenject;

namespace Alensia.Core.Game
{
    public class Game : BaseObject, IGame
    {
        public float TimeScale
        {
            get { return _timeScale.Value; }
            set { _timeScale.Value = value; }
        }

        public bool Paused => _paused.Value;

        public UniRx.IObservable<float> OnTimeScaleChange => _timeScale;

        public UniRx.IObservable<bool> OnPauseStateChange => _paused;

        public UniRx.IObservable<Unit> OnPause =>
            OnPauseStateChange.Where(s => s).AsSingleUnitObservable();

        public UniRx.IObservable<Unit> OnResume =>
            OnPauseStateChange.Where(s => !s).AsSingleUnitObservable();

        private readonly IReactiveProperty<float> _timeScale;

        private readonly IReactiveProperty<bool> _paused;

        public Game([InjectOptional] Settings settings)
        {
            settings = settings ?? new Settings();

            Time.timeScale = settings.TimeScale;

            _timeScale = new ReactiveProperty<float>(Time.timeScale);
            _paused = new ReactiveProperty<bool>();

            _timeScale
                .Where(_ => !Paused)
                .Subscribe(scale => Time.timeScale = scale).AddTo(this);
            _paused
                .Select(v => v ? 0 : settings.TimeScale)
                .Subscribe(scale => Time.timeScale = scale)
                .AddTo(this);
        }

        public void Pause() => _paused.Value = true;

        public void Resume() => _paused.Value = false;
    }

    [Serializable]
    public class Settings : IEditorSettings
    {
        public float TimeScale = 1;
    }
}