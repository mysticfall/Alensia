namespace Alensia.Core.I18n
{
    public interface IMessages
    {
        bool Contains(string key);

        string this[string key] { get; }
    }
}