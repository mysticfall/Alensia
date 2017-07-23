using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using UEButton = UnityEngine.UI.Button;

namespace Alensia.Core.UI
{
    public class Button : Label
    {
        public IObservable<Unit> OnClick => PeerButton?.onClick?.AsObservable();

        protected UEButton PeerButton => _peerButton;

        protected Image PeerImage => _peerImage;

        protected override IList<Component> Peers
        {
            get
            {
                var peers = base.Peers;

                peers.Add(PeerButton);
                peers.Add(PeerImage);

                return peers;
            }
        }

        protected override string DefaultText => "Button";

        [SerializeField, HideInInspector] private UEButton _peerButton;

        [SerializeField, HideInInspector] private Image _peerImage;

        protected override void InitializePeers()
        {
            base.InitializePeers();

            _peerButton = GetComponentInChildren<UEButton>();
            _peerImage = GetComponentInChildren<Image>();
        }

        public new static Button CreateInstance()
        {
            var prefab = Resources.Load<GameObject>("UI/Components/Button");

            Assert.IsNotNull(prefab, "prefab != null");

            return Instantiate(prefab).GetComponent<Button>();
        }
    }
}