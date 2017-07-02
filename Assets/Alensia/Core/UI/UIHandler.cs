using System.Linq;
using UniRx;
using UniRx.Triggers;

namespace Alensia.Core.UI
{
    public abstract class UIHandler : UIElement, IUIHandler
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

        public override void Initialize(IUIContext context)
        {
            base.Initialize(context);

            GetComponentsInChildren<IUIElement>(true)
                .Where(c => !ReferenceEquals(c, this))
                .ToList()
                .ForEach(c => c.Initialize(context));
        }

        public virtual void Close() => Destroy(GameObject);
    }
}