namespace Alensia.Core.UI.Cursor
{
    public interface ICursorManager
    {
        ICursorSet CursorSet { get; set; }

        string DefaultCursor { get; set; }
    }
}