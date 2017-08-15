using System.Collections.Generic;
using Alensia.Core.UI.Property;
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

        public ImageAndColor Background
        {
            get { return _background.Value; }
            set
            {
                Assert.IsNotNull(value, "value != null");

                _background.Value = value;
            }
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

        [SerializeField] private ImageAndColorReactiveProperty _background;

        [SerializeField, HideInInspector] private UEButton _peerButton;

        [SerializeField, HideInInspector] private Image _peerImage;

        public override void Initialize(IUIContext context)
        {
            base.Initialize(context);

            _interactable
                .Subscribe(v => PeerButton.interactable = v)
                .AddTo(this);
            _background
                .Subscribe(b => b.Update(PeerImage))
                .AddTo(this);
        }

        protected override void InitializePeers()
        {
            base.InitializePeers();

            _peerButton = GetComponentInChildren<UEButton>();
            _peerImage = GetComponentInChildren<Image>();
        }

        protected override void ValidateProperties()
        {
            base.ValidateProperties();

            PeerButton.interactable = _interactable.Value;

            Background.Update(PeerImage);
        }

        protected override void Reset()
        {
            base.Reset();

            var source = CreateInstance();

            Interactable = source.Interactable;

            Background.Load(source.PeerImage);
            Background.Update(PeerImage);

            TextStyle.Load(source.PeerText);
            TextStyle.Update(PeerText);

            DestroyImmediate(source.gameObject);
        }

        public new static Button CreateInstance()
        {
            var prefab = Resources.Load<GameObject>("UI/Components/Button");

            Assert.IsNotNull(prefab, "prefab != null");

            return Instantiate(prefab).GetComponent<Button>();
        }
    }
}