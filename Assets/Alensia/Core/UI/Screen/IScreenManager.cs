using System.Collections.Generic;
using Zenject;

namespace Alensia.Core.UI.Screen
{
    public interface IScreenManager : IInitializable
    {
        IReadOnlyList<IScreen> Screens { get; }

        IScreen FindScreen(string name);

        void ShowScreen(string name);

        void HideScreen(string name);
    }
}