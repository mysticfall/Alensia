using System;
using UnityEngine.EventSystems;

namespace Alensia.Core.Interaction.Event
{
    public interface IPointerSelectionAware
    {
        IObservable<PointerEventData> OnPointerPress { get; }

        IObservable<PointerEventData> OnPointerRelease { get; }

        IObservable<PointerEventData> OnPointerSelect { get; }
    }
}