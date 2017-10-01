using System;
using UnityEngine.EventSystems;

namespace Alensia.Core.UI.Event
{
    public interface IPointerPresenceAware
    {
        IObservable<PointerEventData> OnPointerEnter { get; }

        IObservable<PointerEventData> OnPointerExit { get; }
    }
}