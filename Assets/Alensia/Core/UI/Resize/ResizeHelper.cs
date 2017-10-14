using System;
using System.Collections.Generic;
using System.Linq;
using Alensia.Core.Common;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Alensia.Core.UI.Resize
{
    public class ResizeHelper : ActivatableObject
    {
        public readonly IComponent Target;

        public readonly ResizeDirections Directions;

        public Transform HandleParent { get; private set; }

        public ICollection<ResizeHandle> Handles { get; private set; }

        public ResizeHelper(IComponent target, ResizeDirections directions = ResizeDirections.All)
        {
            Assert.IsNotNull(target, "target != null");

            Target = target;
            Directions = directions;

            Handles = Enumerable.Empty<ResizeHandle>().ToList();
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            HandleParent = CreateHandleParent(Target.Transform);

            var handles = new List<ResizeHandle>(8);

            switch (Directions)
            {
                case ResizeDirections.All:
                    handles.Add(CreateHandle<ResizeHandleTop>(HandleParent));
                    handles.Add(CreateHandle<ResizeHandleTopRight>(HandleParent));
                    handles.Add(CreateHandle<ResizeHandleRight>(HandleParent));
                    handles.Add(CreateHandle<ResizeHandleBottomRight>(HandleParent));
                    handles.Add(CreateHandle<ResizeHandleBottom>(HandleParent));
                    handles.Add(CreateHandle<ResizeHandleBottomLeft>(HandleParent));
                    handles.Add(CreateHandle<ResizeHandleLeft>(HandleParent));
                    handles.Add(CreateHandle<ResizeHandleTopLeft>(HandleParent));

                    break;
                case ResizeDirections.CornersOnly:
                    handles.Add(CreateHandle<ResizeHandleTopRight>(HandleParent));
                    handles.Add(CreateHandle<ResizeHandleBottomRight>(HandleParent));
                    handles.Add(CreateHandle<ResizeHandleBottomLeft>(HandleParent));
                    handles.Add(CreateHandle<ResizeHandleTopLeft>(HandleParent));

                    break;
                case ResizeDirections.EdgesOnly:
                    handles.Add(CreateHandle<ResizeHandleTop>(HandleParent));
                    handles.Add(CreateHandle<ResizeHandleRight>(HandleParent));
                    handles.Add(CreateHandle<ResizeHandleBottom>(HandleParent));
                    handles.Add(CreateHandle<ResizeHandleLeft>(HandleParent));

                    break;
                case ResizeDirections.BottomRightOnly:
                    handles.Add(CreateHandle<ResizeHandleBottomRight>(HandleParent));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            Handles = handles;
        }

        protected virtual Transform CreateHandleParent(Transform parent)
        {
            var handles = new GameObject("ResizeHandles", typeof(RectTransform), typeof(LayoutElement));

            handles.transform.SetParent(parent);

            handles.GetComponent<LayoutElement>().ignoreLayout = true;

            var rectTransform = handles.GetComponent<RectTransform>();

            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = new Vector2(1, 1);

            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;

            return handles.transform;
        }

        protected virtual T CreateHandle<T>(Transform parent) where T : ResizeHandle
        {
            var go = new GameObject(typeof(T).Name, typeof(T));

            var handle = go.GetComponent<T>();

            handle.Transform.SetParent(parent);
            handle.Target = Target;
            handle.Interactable = false;

            handle.Initialize(Target.Context);

            return handle;
        }

        protected override void OnActivated()
        {
            base.OnActivated();

            foreach (var handle in Handles)
            {
                handle.Interactable = true;
            }
        }

        protected override void OnDeactivated()
        {
            base.OnDeactivated();

            foreach (var handle in Handles)
            {
                handle.Interactable = false;
            }
        }

        protected override void OnDisposed()
        {
            base.OnDisposed();

            foreach (var handle in Handles)
            {
                Object.Destroy(handle);
            }

            Object.Destroy(HandleParent);

            HandleParent = null;

            Handles = Enumerable.Empty<ResizeHandle>().ToList();
        }
    }

    public enum ResizeDirections
    {
        All,
        CornersOnly,
        EdgesOnly,
        BottomRightOnly
    }
}