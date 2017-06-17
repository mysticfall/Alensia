namespace Alensia.Core.UI.Cursor
{
    public interface ICursorSet
    {
        bool Contains(string key);

        ICursorDefinition this[string key] { get; }
    }
}