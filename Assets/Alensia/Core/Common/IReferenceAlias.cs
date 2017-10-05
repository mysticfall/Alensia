using System;

namespace Alensia.Core.Common
{
    public interface IReferenceAlias<T> : INamed
    {
        T Reference { get; set; }

        IObservable<T> OnChange { get; }
    }
}