using System;
using System.Linq;
using System.Threading.Tasks;
using Alensia.Core.Common;
using Alensia.Core.UI.Screen;
using Optional;
using UniRx;
using UnityEngine;

namespace Alensia.Core.UI
{
    public abstract class Dialog<T> : Window
    {
        protected abstract Option<T> Value { get; }

        public Transform ButtonPanel => _buttons ?? (_buttons = Transform.Find("Buttons"));

        protected Button OkButton =>
            _okButton ?? (_okButton = ButtonPanel.FindComponent<Button>("ButtonOk"));

        protected Button CloseButton =>
            _closeButton ?? (_closeButton = ButtonPanel.FindComponent<Button>("ButtonClose"));

        [SerializeField, HideInInspector] private Button _okButton;

        [SerializeField, HideInInspector] private Button _closeButton;

        [NonSerialized] private Transform _buttons;

        private bool _selected;

        protected override void InitializeComponent(IUIContext context, bool isPlaying)
        {
            base.InitializeComponent(context, isPlaying);

            if (!isPlaying) return;

            _selected = false;

            OkButton?.OnPointerSelect
                .Subscribe(_ =>
                {
                    _selected = true;
                    Remove();
                }, Debug.LogError)
                .AddTo(this);
            CloseButton?.OnPointerSelect
                .Subscribe(_ => Remove(), Debug.LogError)
                .AddTo(this);
        }

        protected override void InitializeChildren(IUIContext context)
        {
            base.InitializeChildren(context);

            ButtonPanel?
                .GetChildren<IComponent>()
                .ToList()
                .ForEach(c => c.Initialize(context));
        }

        public async Task<Option<T>> ShowAsync(IScreen screen)
        {
            Show(screen);

            return await OnRemove.Select(_ => Value.NoneWhen(v => !_selected).Flatten());
        }
    }
}