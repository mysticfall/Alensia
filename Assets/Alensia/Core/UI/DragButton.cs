using System;
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
        public IObservable<PointerEventData> OnDragBegin => this.OnBeginDragAsObservable().Where(_ => Interactable);

        public IObservable<PointerEventData> OnDrag => this.OnDragAsObservable().Where(_ => Interactable);

        public IObservable<PointerEventData> OnDragEnd => this.OnEndDragAsObservable().Where(_ => Interactable);

        protected override TextStyleSet DefaultTextStyleSet =>
            Style?.TextStyleSets?["DragButton.Text"]?.Merge(base.DefaultTextStyleSet) ??
            base.DefaultTextStyleSet;

        protected override ImageAndColorSet DefaultBackgroundSet =>
            Style?.ImageAndColorSets?["DragButton.Background"]?.Merge(base.DefaultBackgroundSet) ??
            base.DefaultBackgroundSet;

        protected override ImageAndColorSet DefaultIconSet =>
            Style?.ImageAndColorSets?["DragButton.Icon"]?.Merge(base.DefaultIconSet) ??
            base.DefaultIconSet;

        protected override void InitializeComponent(IUIContext context, bool isPlaying)
        {
            base.InitializeComponent(context, isPlaying);

            if (!isPlaying) return;

            OnInteractingStateChange
                .Subscribe(UpdateCursor, Debug.LogError)
                .AddTo(this);
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