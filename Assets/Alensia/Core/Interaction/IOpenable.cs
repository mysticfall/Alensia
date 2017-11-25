using System;

namespace Alensia.Core.Interaction
{
    public interface IOpenable : IOpenableOnce, ICloseableOnce
    {
        IObservable<bool> OnOpenStateChange { get; }
    }
}