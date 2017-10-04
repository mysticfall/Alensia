using System;
using UnityEngine.EventSystems;

namespace Alensia.Core.Interaction.Event
{
    public interface IPointerDragAware
    {
        IObservable<PointerEventData> OnDragBegin { get; }

        IObservable<PointerEventData> OnDrag { get; }

        IObservable<PointerEventData> OnDragEnd { get; }
    }
}