namespace Alensia.Core.Locomotion
{
    public interface ILocomotive
    {
        ILocomotion Locomotion { get; }
    }

    namespace Generic
    {
        public interface ILocomotive<out T> : ILocomotive where T : ILocomotion
        {
            new T Locomotion { get; }
        }
    }

    public static class LocomotiveExtensions
    {
        public static bool CanMove(this ILocomotive subject) => 
            subject.Locomotion != null && subject.Locomotion.Active;
    }
}