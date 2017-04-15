using NUnit.Framework;

namespace Alensia.Tests
{
    public abstract class TestBase : AssertionHelper
    {
        public const float Tolerance = 0.01f;
    }
}