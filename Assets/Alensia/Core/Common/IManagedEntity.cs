using UnityEngine;

namespace Alensia.Core.Common
{
    public interface IManagedEntity
    {
        GameObject GameObject { get; }
    }

    public static class ManagedEntityExtensions
    {
        public static bool IsDestroyed(this IManagedEntity entity) => IsDestroyed(entity.GameObject);

        public static bool IsDestroyed(this GameObject gameObject)
        {
            // This works because Unity overrides '==' for GameObject.
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            return gameObject == null && !ReferenceEquals(gameObject, null);
        }
    }
}