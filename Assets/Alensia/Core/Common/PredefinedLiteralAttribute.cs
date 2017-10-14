using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace Alensia.Core.Common
{
    public class PredefinedLiteralAttribute : PropertyAttribute
    {
        public readonly Type Type;

        public readonly bool Optional;

        public readonly bool AllowCustom;

        public PredefinedLiteralAttribute(
            Type type, bool optional = true, bool allowCustom = true)
        {
            Assert.IsNotNull(type, "type != null");

            Type = type;
            Optional = optional;
            AllowCustom = allowCustom;
        }
    }
}