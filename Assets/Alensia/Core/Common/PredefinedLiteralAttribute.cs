using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace Alensia.Core.Common
{
    public class PredefinedLiteralAttribute : PropertyAttribute
    {
        public Type Type;

        public bool Optional;

        public bool AllowCustom;

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