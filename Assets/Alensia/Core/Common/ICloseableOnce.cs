using UniRx;

namespace Alensia.Core.Common
{
    public interface ICloseableOnce
    {
        bool Closed { get; set; }

        IObservable<Unit> OnClose { get; }

        void Close();
    }
}