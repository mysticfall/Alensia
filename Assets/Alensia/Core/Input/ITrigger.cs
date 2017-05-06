namespace Alensia.Core.Input
{
    public interface ITrigger
    {
        bool Up { get; }

        bool Down { get; }

        bool Hold { get; }
    }
}