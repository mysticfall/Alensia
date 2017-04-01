namespace Alensia.Core.Physics
{
    public interface IGroundDetector
    {
        void OnHitGround();

        void OnLeaveGround();
    }
}