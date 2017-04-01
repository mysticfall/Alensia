namespace Alensia.Core.Common
{
    public interface IActivatable
    {
        bool Active { get; }

        void Activate();

        void Deactivate();
    }
}