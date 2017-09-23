using System;
using System.Collections.Generic;
using System.Linq;
using Alensia.Core.Common;
using UnityEngine;
using UnityEngine.Assertions;
using Object = UnityEngine.Object;

namespace Alensia.Core.UI.Resize
{
    public class ResizeHelper : BaseActivatable
    {
        public readonly IComponent Target;

        public readonly ResizeDirections Directions;

        public Transform HandleParent => _handleParent;

        public ICollection<ResizeHandle> Handles { get; private set; }

        private Transform _handleParent;

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

            var parent = new GameObject("ResizeHandles");

            _handleParent = parent.transform;
            _handleParent.SetParent(Target.Transform);

            var handles = new List<ResizeHandle>(8);

            switch (Directions)
            {
                case ResizeDirections.All:
                    handles.Add(CreateHandle<ResizeHandleTop>());
                    handles.Add(CreateHandle<ResizeHandleTopRight>());
                    handles.Add(CreateHandle<ResizeHandleRight>());
                    handles.Add(CreateHandle<ResizeHandleBottomRight>());
                    handles.Add(CreateHandle<ResizeHandleBottom>());
                    handles.Add(CreateHandle<ResizeHandleBottomLeft>());
                    handles.Add(CreateHandle<ResizeHandleLeft>());
                    handles.Add(CreateHandle<ResizeHandleTopLeft>());

                    break;
                case ResizeDirections.CornersOnly:
                    handles.Add(CreateHandle<ResizeHandleTopRight>());
                    handles.Add(CreateHandle<ResizeHandleBottomRight>());
                    handles.Add(CreateHandle<ResizeHandleBottomLeft>());
                    handles.Add(CreateHandle<ResizeHandleTopLeft>());

                    break;
                case ResizeDirections.EdgesOnly:
                    handles.Add(CreateHandle<ResizeHandleTop>());
                    handles.Add(CreateHandle<ResizeHandleRight>());
                    handles.Add(CreateHandle<ResizeHandleBottom>());
                    handles.Add(CreateHandle<ResizeHandleLeft>());

                    break;
                case ResizeDirections.BottomRightOnly:
                    handles.Add(CreateHandle<ResizeHandleBottomRight>());
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            handles.ForEach(h => h.transform.SetParent(HandleParent));

            Handles = handles;
        }

        protected virtual T CreateHandle<T>() where T : ResizeHandle
        {
            var go = new GameObject(typeof(T).Name, typeof(T));

            var handle = go.GetComponent<T>();

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

            _handleParent = null;

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