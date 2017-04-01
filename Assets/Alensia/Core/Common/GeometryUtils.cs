namespace Alensia.Core.Common
{
    public class GeometryUtils
    {
        public static float NormalizeAspectAngle(float degrees)
        {
            var value = degrees;

            while (value < 0) value += 360;

            return value > 180 ? value - 360 : value;
        }
    }
}