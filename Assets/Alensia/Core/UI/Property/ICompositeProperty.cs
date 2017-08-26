using Alensia.Core.Common;

namespace Alensia.Core.UI.Property
{
    public interface ICompositeProperty<in TProp, in TSource> : IEditorSettings
        where TProp : ICompositeProperty<TProp, TSource>
    {
        void Update(TSource source);

        void Update(TSource source, TProp defaultValue);
    }
}