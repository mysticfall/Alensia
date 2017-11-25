using System;
using UniRx;

namespace Alensia.Core.Interaction
{
    public interface ICloseableOnce
    {
        bool Closed { get; set; }

        IObservable<Unit> OnClose { get; }

        void Close();
    }
}