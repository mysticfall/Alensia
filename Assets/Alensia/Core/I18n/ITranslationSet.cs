namespace Alensia.Core.I18n
{
    public interface ITranslationSet
    {
        bool Contains(string key);

        string this[string key] { get; }
    }
}