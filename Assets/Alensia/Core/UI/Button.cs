using System.Collections.Generic;
using Alensia.Core.UI.Property;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using UEButton = UnityEngine.UI.Button;

namespace Alensia.Core.UI
{
    public class Button : Label, IInteractableComponent
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

        [SerializeField] private BoolReactiveProperty _interactable;

        [SerializeField] private ImageAndColorReactiveProperty _background;

        [SerializeField, HideInInspector] private UEButton _peerButton;

        [SerializeField, HideInInspector] private Image _peerImage;

        protected override void InitializeProperties(IUIContext context)
        {
            base.InitializeProperties(context);

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

        protected override void UpdateEditor()
        {
            base.UpdateEditor();

            PeerButton.interactable = _interactable.Value;

            Background.Update(PeerImage);
        }

        protected override void ResetFromInstance(UIComponent component)
        {
            base.ResetFromInstance(component);

            var source = (Button) component;

            Interactable = source.Interactable;

            Background = new ImageAndColor(source.Background);
        }

        protected override UIComponent CreatePristineInstance() => CreateInstance();

        public new static Button CreateInstance()
        {
            var prefab = Resources.Load<GameObject>("UI/Components/Button");

            Assert.IsNotNull(prefab, "prefab != null");

            return Instantiate(prefab).GetComponent<Button>();
        }
    }
}