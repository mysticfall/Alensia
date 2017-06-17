namespace Alensia.Core.UI.Cursor
{
    public interface ICursorManager
    {
        ICursorSet CursorSet { get; set; }

        string Cursor { get; }

        string DefaultCursor { get; set; }
    }
}