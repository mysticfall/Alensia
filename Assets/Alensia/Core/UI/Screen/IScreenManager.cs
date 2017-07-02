using System.Collections.Generic;
using UnityEngine;

namespace Alensia.Core.UI.Screen
{
    public interface IScreenManager
    {
        IReadOnlyList<IScreen> Screens { get; }

        Transform ScreenRoot { get; }

        IScreen FindScreen(string name);

        void ShowScreen(string name);

        void HideScreen(string name);
    }
}