using Alensia.Core.UI.Event;
using Alensia.Core.UI.Property;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UEButton = UnityEngine.UI.Button;
using UECursor = UnityEngine.Cursor;

namespace Alensia.Core.UI
{
    public class DragButton : Button, IPointerDragAware
    {
        public IObservable<PointerEventData> OnDragBegin => this.OnBeginDragAsObservable();

        public IObservable<PointerEventData> OnDrag => this.OnDragAsObservable();

        public IObservable<PointerEventData> OnDragEnd => this.OnEndDragAsObservable();

        protected override ImageAndColorSet DefaultBackgroundSet =>
            Style?.ImageAndColorSets?["DragButton.Background"]?.Merge(base.DefaultBackgroundSet) ??
            base.DefaultBackgroundSet;

        protected override void InitializeProperties(IUIContext context)
        {
            base.InitializeProperties(context);

            OnInteractingStateChange.Subscribe(UpdateCursor).AddTo(this);
        }

        private static void UpdateCursor(bool interacting)
        {
            UECursor.visible = !interacting;

            //TODO Don't lock cursor, since Unity will reset cursor position upon unlocking.   
            //UECursor.lockState = interacting ? CursorLockMode.Locked : CursorLockMode.None;
        }

        protected override EventTracker<UEButton> CreateInterationTracker() =>
            new PointerDragTracker<UEButton>(PeerHotspot);

        protected override UIComponent CreatePristineInstance() => CreateInstance();

        public new static DragButton CreateInstance()
        {
            var prefab = Resources.Load<GameObject>("UI/Components/DragButton");

            Assert.IsNotNull(prefab, "prefab != null");

            return Instantiate(prefab).GetComponent<DragButton>();
        }
    }
}