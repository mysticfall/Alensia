using System;

namespace Alensia.Core.UI
{
    public interface IStylable
    {
        UIStyle Style { get; set; }

        IObservable<UIStyle> OnStyleChange { get; }
    }
}