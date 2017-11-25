using System;
using UniRx;

namespace Alensia.Core.Interaction
{
    public interface IOpenableOnce
    {
        bool Opened { get; set; }

        IObservable<Unit> OnOpen { get; }

        void Open();
    }
}