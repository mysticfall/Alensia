using System;

namespace Alensia.Core.UI.Event
{
    public interface IInteractable
    {
        bool Interactable { get; set; }

        bool Interacting { get; }

        IObservable<bool> OnInteractableStateChange { get; }

        IObservable<bool> OnInteractingStateChange { get; }
    }
}