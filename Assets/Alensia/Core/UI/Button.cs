using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using UEButton = UnityEngine.UI.Button;

namespace Alensia.Core.UI
{
    [RequireComponent(typeof(UEButton), typeof(Image))]
    public class Button : Label
    {
        public IObservable<Unit> OnClick => PeerButton?.onClick?.AsObservable();

        protected UEButton PeerButton { get; private set; }

        protected override void OnValidate()
        {
            base.OnValidate();

            PeerButton = PeerButton ?? GetComponent<UEButton>();

            Assert.IsNotNull(PeerButton, "Missing Button component.");
        }

        public new static Button CreateInstance()
        {
            var prefab = Resources.Load<GameObject>("UI/Components/Button");

            Assert.IsNotNull(prefab, "prefab != null");

            return Instantiate(prefab).GetComponent<Button>();
        }
    }
}