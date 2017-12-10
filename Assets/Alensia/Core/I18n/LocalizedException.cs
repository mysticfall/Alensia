using System;

namespace Alensia.Core.I18n
{
    public class LocalizedException : Exception, ITranslatable
    {
        public TranslatableText LocalizedMessage { get; }

        public LocalizedException(
            TranslatableText message, Exception innerException = null) : base(message?.Text, innerException)
        {
            LocalizedMessage = message;
        }

        public string Translate(ITranslator translator) => LocalizedMessage?.Translate(translator);
    }
}