using Alensia.Core.Character.Morph;
using Alensia.Core.I18n;
using Alensia.Core.UI;
using UMA.CharacterSystem;
using UniRx;

namespace Alensia.Demo.UMA
{
    public class MorphSlider : Panel
    {
        public RangedMorph<float> Morph
        {
            get { return _morph; }
            set
            {
                if (Equals(_morph, value)) return;

                _morph = value;

                if (Context != null)
                {
                    UpdateMorph();
                }
            }
        }

        public Label Label { get; private set; }

        public Slider Slider { get; private set; }

        private RangedMorph<float> _morph;

        public override void Initialize(IUIContext context)
        {
            base.Initialize(context);

            UpdateMorph();
        }

        protected override void InitializePeers()
        {
            base.InitializePeers();

            Label = GetComponentInChildren<Label>();
            Slider = GetComponentInChildren<Slider>();

            Slider.OnValueChange
                .Where(_ => Morph != null)
                .Subscribe(v => Morph.Value = v)
                .AddTo(this);
        }

        protected virtual void UpdateMorph()
        {
            if (Morph == null)
            {
                Label.Text = new TranslatableText("(none)");

                Slider.MinValue = 0;
                Slider.MaxValue = 1;
                Slider.Value = 0;
            }
            else
            {
                var displanName = Morph.Name.BreakupCamelCase();

                Label.Text = new TranslatableText(displanName);

                Slider.MinValue = Morph.MinValue;
                Slider.MaxValue = Morph.MaxValue;
                Slider.Value = Morph.Value;
            }
        }
    }
}