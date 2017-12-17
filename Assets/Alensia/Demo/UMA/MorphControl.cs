using Alensia.Core.Character.Morph;
using Alensia.Core.I18n;
using Zenject;

namespace Alensia.Demo.UMA
{
    public abstract class MorphControl<T> : ControlPanel where T : IMorph
    {
        [Inject]
        public IMorphNameResolver NameResolver { get; }

        public T Morph
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

        private T _morph;

        protected virtual void UpdateMorph()
        {
            var text = Morph == null ? "(none)" : NameResolver.Resolve(Morph.Name);

            Label.Text = new TranslatableText(text);
        }
    }
}