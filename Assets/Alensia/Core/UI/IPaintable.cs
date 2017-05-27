using System;

namespace Alensia.Core.UI
{
    public interface IPaintable
    {
        void Paint();

        void OnNextPaint(Action action);
    }
}