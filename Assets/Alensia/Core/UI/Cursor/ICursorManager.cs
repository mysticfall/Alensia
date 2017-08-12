namespace Alensia.Core.UI.Cursor
{
    public interface ICursorManager
    {
        CursorState CursorState { get; set; }

        ICursorSet CursorSet { get; set; }

        string DefaultCursor { get; set; }
    }
}