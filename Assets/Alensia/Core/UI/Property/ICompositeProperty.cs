using Alensia.Core.Common;
using UniRx;

namespace Alensia.Core.UI.Property
{
    public interface ICompositeProperty<TProp, in TValue> : IEditorSettings 
        where TProp : ICompositeProperty<TProp, TValue>
    {
        IObservable<TProp> OnChange { get; }

        void Load(TValue value);

        void Update(TValue value);
    }
}