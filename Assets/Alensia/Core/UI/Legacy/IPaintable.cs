using System;

namespace Alensia.Core.UI.Legacy
{
    public interface IPaintable
    {
        void Paint();

        void OnNextPaint(Action action);
    }
}