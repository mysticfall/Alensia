using Alensia.Core.I18n;

namespace Alensia.Core.Common
{
    public interface ILabelled : INamed
    {
        TranslatableText DisplayName { get; }
    }
}