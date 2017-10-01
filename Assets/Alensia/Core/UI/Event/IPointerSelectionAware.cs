using System;
using UnityEngine.EventSystems;

namespace Alensia.Core.UI.Event
{
    public interface IPointerSelectionAware
    {
        IObservable<PointerEventData> OnPointerPress { get; }

        IObservable<PointerEventData> OnPointerRelease { get; }

        IObservable<PointerEventData> OnPointerSelect { get; }
    }
}