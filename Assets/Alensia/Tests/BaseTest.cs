using NUnit.Framework;

namespace Alensia.Tests
{
    public abstract class BaseTest : AssertionHelper
    {
        public const float Tolerance = 0.0001f;
    }
}