using Alensia.Core.Common;
using Alensia.Core.Geom;
using Alensia.Core.I18n;

namespace Alensia.Core.Entity
{
    public interface IEntity : INamed, ITransformable
    {
        TranslatableText DisplayName { get; }
    }
}