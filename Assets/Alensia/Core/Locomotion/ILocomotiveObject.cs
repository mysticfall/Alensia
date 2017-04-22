namespace Alensia.Core.Locomotion
{
    public interface ILocomotiveObject<out T> where T : ILocomotion
    {
        T Locomotion { get; }
    }
}