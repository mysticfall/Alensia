using System;
using System.Linq;
using Alensia.Core.Interaction;

namespace Alensia.Core.UI
{
    public interface IInteractableComponent : IComponent, IHighlightable, IInteractable
    {
        string Cursor { get; }

        IObservable<string> OnCursorChange { get; }
    }

    public static class InteractableComponentExtensions
    {
        public static bool HasActiveChild(this IInteractableComponent component)
        {
            var active = (component.Context as IRuntimeUIContext)?.ActiveComponent;

            return active?.Ancestors.FirstOrDefault(c => ReferenceEquals(c, component)) != null;
        }

        public static IInteractableComponent FindFirstActiveAncestor(this IInteractableComponent component)
        {
            var ancestor = component.Ancestors.FirstOrDefault(a => a is IInteractableComponent && a.Visible);

            return ancestor as IInteractableComponent;
        }
    }
}