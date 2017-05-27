using UnityEngine;

namespace Alensia.Core.UI
{
    public interface IUIManager : IComponentsHolder
    {
        GUISkin Skin { get; set; }
    }
}