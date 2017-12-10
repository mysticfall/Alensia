using Alensia.Core.Character.Morph;
using Alensia.Core.I18n;
using Alensia.Core.UI;
using UMA.CharacterSystem;
using UnityEngine;

namespace Alensia.Demo.UMA
{
    public abstract class MorphControl : Panel
    {
        protected Label Label => _label ?? FindPeer<Label>("Label");

        [SerializeField, HideInInspector] private Label _label;
    }

    namespace Generic
    {
        public abstract class MorphControl<T> : MorphControl where T : IMorph
        {
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
                var text = Morph == null ? "(none)" : Morph.Name.BreakupCamelCase();

                Label.Text = new TranslatableText(text);
            }
        }
    }
}