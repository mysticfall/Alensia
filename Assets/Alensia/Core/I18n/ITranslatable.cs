namespace Alensia.Core.I18n
{
    public interface ITranslatable
    {
        string Translate(ITranslator translator);
    }
}