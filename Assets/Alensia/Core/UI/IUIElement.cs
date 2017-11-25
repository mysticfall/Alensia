using System;
using Alensia.Core.Common;
using Alensia.Core.Geom;
using Alensia.Core.Interaction;
using UniRx;
using UnityEngine;

namespace Alensia.Core.UI
{
    public interface IUIElement : IUIContextHolder, INamed, IHideable, IValidatable, ITransformable
    {
        RectTransform RectTransform { get; }

        IObservable<Unit> OnRemove { get; }

        void Initialize(IUIContext context);

        void Remove();
    }
}