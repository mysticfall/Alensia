using UnityEngine;
using UnityEngine.AI;

namespace Alensia.Core.Navigation
{
    public class TransformNavigator : Navigator
    {
        public TransformNavigator(
            NavMeshAgent agent,
            NavigatorSettings settings,
            Transform transform) : base(agent, settings, transform)
        {
            Agent.updatePosition = false;
            Agent.updateRotation = false;
            Agent.updateUpAxis = false;
        }

        protected override void OnDestinationChange(Vector3? destination)
        {
        }

        protected override void UpdatePosition()
        {
            if (Destination != null)
            {
                var delta = Destination.Value - Transform.position;
                var speed = Settings.ForwardSpeed;

                Transform.position = Vector3.MoveTowards(
                    Transform.position, Destination.Value, speed * Time.deltaTime);
            }

            if (TargetRotation != null && !FaceDestination)
            {
                //var rotation = Quaternion.FromToRotation(Transform.forward, direction);
            }
        }
    }
}