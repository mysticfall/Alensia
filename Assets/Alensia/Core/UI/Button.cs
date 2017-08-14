using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using UEButton = UnityEngine.UI.Button;

namespace Alensia.Core.UI
{
    public class Button : Label, IControl
    {
        public bool Interactable
        {
            get { return _interactable.Value; }
            set { _interactable.Value = value; }
        }

        public IObservable<Unit> OnClick => PeerButton?.onClick.AsObservable().Where(_ => Interactable);

        protected UEButton PeerButton => _peerButton;

        protected Image PeerImage => _peerImage;

        protected override IList<Object> Peers
        {
            get
            {
                var peers = base.Peers;

                if (PeerButton != null) peers.Add(PeerButton);
                if (PeerImage != null) peers.Add(PeerImage);

                if (PeerText != null) peers.Add(PeerText.gameObject);

                return peers;
            }
        }

        protected override string DefaultText => "Button";

        [SerializeField] private BoolReactiveProperty _interactable;

        [SerializeField, HideInInspector] private UEButton _peerButton;

        [SerializeField, HideInInspector] private Image _peerImage;

        protected override void InitializePeers()
        {
            base.InitializePeers();

            _peerButton = GetComponentInChildren<UEButton>();
            _peerImage = GetComponentInChildren<Image>();

            _interactable.Value = _peerButton.IsInteractable();
        }

        protected override void ValidateProperties()
        {
            base.ValidateProperties();

            PeerButton.interactable = _interactable.Value;
        }

        public override void Initialize(IUIContext context)
        {
            base.Initialize(context);

            _interactable
                .Subscribe(v => PeerButton.interactable = v)
                .AddTo(this);
        }

        public new static Button CreateInstance()
        {
            var prefab = Resources.Load<GameObject>("UI/Components/Button");

            Assert.IsNotNull(prefab, "prefab != null");

            return Instantiate(prefab).GetComponent<Button>();
        }
    }
}