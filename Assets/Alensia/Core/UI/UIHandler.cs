using UniRx;
using UniRx.Triggers;

namespace Alensia.Core.UI
{
    public abstract class UIHandler<T> : ComponentHandler<T>, IUIHandler
        where T : class, IComponent
    {
        public bool Closed
        {
            get { return IsDestroyed(); }
            set
            {
                if (value) Close();
            }
        }

        public IObservable<Unit> OnClose => this.OnDestroyAsObservable();

        public virtual void Close() => Destroy(GameObject);
    }
}