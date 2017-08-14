using Alensia.Core.Common;

namespace Alensia.Core.UI.Property
{
    public interface ICompositeProperty<TProp, in TValue> : IEditorSettings 
        where TProp : ICompositeProperty<TProp, TValue>
    {
        void Load(TValue value);

        void Update(TValue value);
    }
}