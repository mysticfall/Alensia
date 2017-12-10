using Alensia.Core.Character.Morph.Generic;
using Alensia.Core.UI;
using Alensia.Core.UI.Property;
using Alensia.Demo.UMA.Generic;
using UniRx;
using UnityEngine;

namespace Alensia.Demo.UMA
{
    public class ColorItem : MorphControl<IMorph<Color>>
    {
        protected ImageButton Button => _button ?? FindPeer<ImageButton>("Button");

        [SerializeField, HideInInspector] private ImageButton _button;

        protected override void InitializeComponent(IUIContext context, bool isPlaying)
        {
            base.InitializeComponent(context, isPlaying);

            if (!isPlaying) return;

            Button.OnPointerSelect
                .Where(_ => Morph != null)
                .Subscribe(_ => ShowColorChooser(), Debug.LogError)
                .AddTo(this);
        }

        protected override void OnStyleChanged(UIStyle style)
        {
            base.OnStyleChanged(style);

            var background = style?.ImageAndColorSets?["Button.Background"];

            if (background != null)
            {
                Button.Background = Button.Background
                    .WithHighlightedValue(background.Normal)
                    .WithActiveValue(background.Active);
            }
        }

        public virtual async void ShowColorChooser()
        {
            var chooser = ColorChooserDialog.CreateInstance();

            chooser.Initialize(Context);
            chooser.Chooser.Value = Morph.Value;

            var color = await chooser.ShowAsync(Screen);

            color.MatchSome(c=>
            {
                Morph.Value = c;
                UpdateMorph();
            });
        }

        protected override void UpdateMorph()
        {
            base.UpdateMorph();

            var color = Morph?.Value ?? Color.clear;

            var icon = Button.Icon.Normal;
            var newIcon = new ImageAndColor(new UnsettableColor(color), icon.Image, icon.Type);

            Button.Icon = new ImageAndColorSet(newIcon, newIcon, newIcon, newIcon);
        }
    }
}