using UnityEngine;

namespace Alensia.Core.Common
{
    public interface IGameObject
    {
        GameObject GameObject { get; }
    }

    public static class GameObjectExtensions
    {
        public static bool IsDestroyed(this IGameObject gameObject) => IsDestroyed(gameObject.GameObject);

        public static bool IsDestroyed(this GameObject gameObject)
        {
            // This works because Unity overrides '==' for GameObject.
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            return gameObject == null && !ReferenceEquals(gameObject, null);
        }
    }
}