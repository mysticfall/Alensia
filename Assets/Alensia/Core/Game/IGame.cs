using UniRx;

namespace Alensia.Core.Game
{
    public interface IGame
    {
        float TimeScale { get; set; }

        bool Paused { get; }

        IObservable<float> OnTimeScaleChange { get; }

        IObservable<bool> OnPauseStateChange { get; }

        IObservable<Unit> OnPause { get; }

        IObservable<Unit> OnResume { get; }

        void Pause();

        void Resume();

        void Quit();
    }
}