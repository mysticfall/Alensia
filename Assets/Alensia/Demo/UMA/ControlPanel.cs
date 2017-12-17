using Alensia.Core.UI;
using UnityEngine;

namespace Alensia.Demo.UMA
{
    public abstract class ControlPanel : Panel
    {
        protected Label Label => _label ?? FindPeer<Label>("Label");

        [SerializeField, HideInInspector] private Label _label;
    }
}