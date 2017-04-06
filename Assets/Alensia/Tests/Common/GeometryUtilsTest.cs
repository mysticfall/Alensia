using Alensia.Core.Common;
using NUnit.Framework;

namespace Alensia.Tests.Common
{
    [TestFixture, Description("Test suite for GeometryUtils.")]
    public class GeometryUtilsTest : BaseTest
    {
        [Test, Description("NormalizeAspectAngle() should normalize a given angle to -180 < v < 180 range.")]
        [TestCase(0, 0)]
        [TestCase(90, 90)]
        [TestCase(180, 180)]
        [TestCase(270, -90)]
        [TestCase(390, 30)]
        [TestCase(-390, -30)]
        public void ShouldNormalizeAspectAngle(float input, float expected)
        {
            Expect(
                GeometryUtils.NormalizeAspectAngle(input),
                Is.EqualTo(expected),
                "NormalizeAspectAngle() returned an unexpected value.");
        }
    }
}