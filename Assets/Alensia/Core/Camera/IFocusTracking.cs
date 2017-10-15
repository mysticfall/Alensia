using System;
using UnityEngine;

namespace Alensia.Core.Camera
{
    public interface IFocusTracking
    {
        Transform Focus { get; }

        IObservable<Transform> OnFocusChange { get; }
    }
}