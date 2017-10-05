using System;
using UniRx;

namespace Alensia.Core.I18n
{
    [Serializable]
    public class LanguageTagReactiveProperty : ReactiveProperty<LanguageTag>
    {
        public LanguageTagReactiveProperty()
        {
        }

        public LanguageTagReactiveProperty(LanguageTag initialValue) : base(initialValue)
        {
        }

        public LanguageTagReactiveProperty(IObservable<LanguageTag> source) : base(source)
        {
        }

        public LanguageTagReactiveProperty(
            IObservable<LanguageTag> source, LanguageTag initialValue) : base(source, initialValue)
        {
        }
    }
}