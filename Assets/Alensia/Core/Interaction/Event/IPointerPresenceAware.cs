using System;
using UnityEngine.EventSystems;

namespace Alensia.Core.Interaction.Event
{
    public interface IPointerPresenceAware
    {
        IObservable<PointerEventData> OnPointerEnter { get; }

        IObservable<PointerEventData> OnPointerExit { get; }
    }
}