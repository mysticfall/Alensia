using System;
using UniRx;

namespace Alensia.Core.Interaction
{
    public interface IActivatable
    {
        bool Active { get; set; }

        void Activate();

        void Deactivate();

        IObservable<Unit> OnActivate { get; }

        IObservable<Unit> OnDeactivate { get; }

        IObservable<bool> OnActiveStateChange { get; }
    }
}