using UniRx;

namespace Alensia.Core.Common
{
    public interface IOpenableOnce
    {
        bool Opened { get; set; }

        IObservable<Unit> OnOpen { get; }

        void Open();
    }
}