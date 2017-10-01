using System;
using Alensia.Core.Common;

namespace Alensia.Core.I18n
{
    [Serializable]
    public class ResourceSettings : IEditorSettings
    {
        public string[] Resources = {"Translations/Messages"};
    }
}