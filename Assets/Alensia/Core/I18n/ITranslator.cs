namespace Alensia.Core.I18n
{
    public interface ITranslator : IMessages
    {
        ILocaleService LocaleService { get; }

        string Translate(string key, params object[] args);
    }
}