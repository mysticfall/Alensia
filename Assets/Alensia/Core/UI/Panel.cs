using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Alensia.Core.UI
{
    [RequireComponent(typeof(Image))]
    public class Panel : UIContainer
    {
        protected Image PeerImage { get; private set; }

        protected override void OnValidate()
        {
            base.OnValidate();

            PeerImage = PeerImage ?? GetComponent<Image>();

            Assert.IsNotNull(PeerImage, "Missing Image component.");
        }

        public static Panel CreateInstance()
        {
            var prefab = Resources.Load<GameObject>("UI/Components/Panel");

            Assert.IsNotNull(prefab, "prefab != null");

            return Instantiate(prefab).GetComponent<Panel>();
        }
    }
}