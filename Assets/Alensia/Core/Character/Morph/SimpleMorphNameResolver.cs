using System.Text.RegularExpressions;
using UnityEngine;

namespace Alensia.Core.Character.Morph
{
    public class SimpleMorphNameResolver : MonoBehaviour, IMorphNameResolver
    {
        public string Resolve(string text) => Capitalize(SplitCamelCase(text)).Trim();

        private static string Capitalize(string text) =>
            text.Length < 2 ? text : text[0].ToString().ToUpper() + text.Substring(1);

        private static string SplitCamelCase(string text) =>
            Regex.Replace(text, "([A-Z])", " $1", RegexOptions.Compiled);
    }
}