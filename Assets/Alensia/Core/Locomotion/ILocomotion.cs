using Alensia.Core.Common;
using UnityEngine;

namespace Alensia.Core.Locomotion
{
    /// <summary>
    /// A character or vehicle which can be moved or rotated by using more intuitive terms
    /// (i.e. "Rotate 60 degrees left" or "Move straight forward".).
    /// </summary>
    public interface ILocomotion : ITransformable
    {
        /// <summary>
        /// Effective velocity of this object.
        /// </summary>
        Vector3 Velocity { get; }

        /// <summary>
        /// Move this object in the given direction at the specified speed.
        /// </summary>
        /// <param name="direction">
        /// Normalized direction in local space to which this object will move.
        /// </param>
        /// <param name="desiredSpeed">
        /// Desired speed of the movement in m/s.
        /// </param>
        void Move(Vector3 direction, float desiredSpeed);

        /// <summary>
        /// Rotate this object in the given direction at the specified angular speed.
        /// </summary>
        /// <param name="rotation">
        /// Amount of rotation in local euler degrees to which this object will turn.
        /// </param>
        /// <param name="desiredSpeed">
        /// Desired angular speed of the rotation in deg/s.
        /// </param>
        void Rotate(Vector3 rotation, float desiredSpeed);
    }
}