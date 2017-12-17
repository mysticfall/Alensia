using Alensia.Core.Character.Morph;
using Alensia.Core.UI;
using UniRx;
using UnityEngine;

namespace Alensia.Demo.UMA
{
    public class MorphSlider : MorphControl<RangedMorph<float>>
    {
        protected Slider Slider => _slider ?? FindPeer<Slider>("Slider");

        [SerializeField, HideInInspector] private Slider _slider;

        protected override void InitializeComponent(IUIContext context, bool isPlaying)
        {
            base.InitializeComponent(context, isPlaying);

            if (!isPlaying) return;

            Slider.OnValueChange
                .Where(_ => Morph != null)
                .Subscribe(v => Morph.Value = v, Debug.LogError)
                .AddTo(this);
        }

        protected override void UpdateMorph()
        {
            base.UpdateMorph();

            if (Morph == null)
            {
                Slider.MinValue = 0;
                Slider.MaxValue = 1;
                Slider.Value = 0;
            }
            else
            {
                Slider.MinValue = Morph.MinValue;
                Slider.MaxValue = Morph.MaxValue;
                Slider.Value = Morph.Value;
            }
        }
    }
}