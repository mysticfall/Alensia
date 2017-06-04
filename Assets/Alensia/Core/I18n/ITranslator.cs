namespace Alensia.Core.I18n
{
    public interface ITranslator
    {
        ILocaleService LocaleService { get; }

        string this[string key] { get; }

        string Translate(string key, params object[] args);
    }
}