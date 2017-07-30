namespace Alensia.Core.Locomotion
{
    public interface ILocomotive<out T> where T : ILocomotion
    {
        T Locomotion { get; }
    }
}