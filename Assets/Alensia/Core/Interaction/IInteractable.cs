using System;

namespace Alensia.Core.Interaction
{
    public interface IInteractable
    {
        bool Interactable { get; set; }

        bool Interacting { get; }

        IObservable<bool> OnInteractableStateChange { get; }

        IObservable<bool> OnInteractingStateChange { get; }
    }
}