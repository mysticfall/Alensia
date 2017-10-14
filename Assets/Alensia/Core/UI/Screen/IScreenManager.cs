using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Alensia.Core.UI.Screen
{
    public interface IScreenManager : IInitializable
    {
        Transform ScreenRoot { get; }

        IEnumerable<IScreen> Screens { get; }

        IScreen FindScreen(string name);

        void ShowScreen(string name);

        void HideScreen(string name);
    }
}