using System;
using Alensia.Core.Common;
using Alensia.Core.Interaction;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace Alensia.Core.UI.Event
{
    public class InteractionHandler<T> : BaseActivatable, IHighlightable, IInteractable, IUIContextHolder
        where T : UIBehaviour
    {
        public readonly IInteractableComponent Component;

        public IUIContext Context => Component?.Context;

        public readonly EventTracker<T> HighlightTracker;

        public readonly EventTracker<T> InteractionTracker;

        public bool Interactable
        {
            get { return Active; }
            set { Active = value; }
        }

        public bool Highlighted => HighlightTracker != null && HighlightTracker.State;

        public bool Interacting => InteractionTracker != null && InteractionTracker.State;

        public IObservable<bool> OnInteractableStateChange => OnActiveStateChange;

        public IObservable<bool> OnInteractingStateChange => InteractionTracker.OnStateChange;

        public IObservable<bool> OnHighlightedStateChange => HighlightTracker.OnStateChange;

        public IObservable<Unit> OnStateChange => OnHighlightedStateChange
            .Merge(OnInteractableStateChange)
            .Merge(OnInteractingStateChange)
            .AsUnitObservable();

        public InteractionHandler(
            IInteractableComponent component,
            EventTracker<T> highlightTracker,
            EventTracker<T> interactionTracker)
        {
            Assert.IsNotNull(component, "component != null");
            Assert.IsNotNull(highlightTracker, "highlightTracker != null");
            Assert.IsNotNull(interactionTracker, "interactionTracker != null");

            Component = component;
            HighlightTracker = highlightTracker;
            InteractionTracker = interactionTracker;
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            HighlightTracker.Initialize();
            InteractionTracker.Initialize();

            OnStateChange
                .Where(_ => (Interacting || Highlighted) && !Component.HasActiveChild())
                .Where(_ => Context.ActiveComponent == null || !Context.ActiveComponent.Interacting)
                .Subscribe(_ => Context.ActiveComponent = Component, Debug.LogError)
                .AddTo(this);

            OnStateChange
                .Merge(Component.OnHide)
                .Where(_ => ReferenceEquals(Context.ActiveComponent, Component))
                .Where(_ => !Interacting && !Highlighted)
                .Subscribe(_ => Context.ActiveComponent = Component.FindFirstActiveAncestor(), Debug.LogError)
                .AddTo(this);
        }

        protected override void OnDisposed()
        {
            HighlightTracker.Dispose();
            InteractionTracker.Dispose();

            if (Context != null && ReferenceEquals(Context.ActiveComponent, Component))
            {
                Context.ActiveComponent = Component.FindFirstActiveAncestor();
            }

            base.OnDisposed();
        }

        protected override void OnActivated()
        {
            base.OnActivated();

            HighlightTracker.Active = true;
            InteractionTracker.Active = true;
        }

        protected override void OnDeactivated()
        {
            HighlightTracker.Active = false;
            InteractionTracker.Active = false;

            base.OnDeactivated();
        }
    }
}