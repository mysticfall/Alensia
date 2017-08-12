using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Alensia.Core.UI
{
    public class Panel : UIContainer
    {
        public ImageProperty Background => _background;

        protected Image PeerImage => _peerImage;

        protected override IList<Component> Peers
        {
            get
            {
                var peers = base.Peers;

                if (PeerImage != null) peers.Add(PeerImage);

                return peers;
            }
        }

        [SerializeField] private ImageProperty _background;

        [SerializeField, HideInInspector] private Image _peerImage;

        protected override void InitializePeers()
        {
            base.InitializePeers();

            _peerImage = GetComponentInChildren<Image>();

            Background.OnChange.Subscribe(i => i.Update(PeerImage)).AddTo(this);
        }

        protected override void ValidateProperties()
        {
            base.ValidateProperties();

            if (PeerImage != null)
            {
                Background.Update(PeerImage);
            }
        }

        protected override void Reset()
        {
            base.Reset();

            var source = CreateInstance();

            Background.Load(source.PeerImage);

            if (PeerImage != null)
            {
                Background.Update(PeerImage);
            }

            DestroyImmediate(source.gameObject);
        }

        public static Panel CreateInstance()
        {
            var prefab = Resources.Load<GameObject>("UI/Components/Panel");

            Assert.IsNotNull(prefab, "prefab != null");

            return Instantiate(prefab).GetComponent<Panel>();
        }
    }
}