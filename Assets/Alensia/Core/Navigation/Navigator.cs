using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;
using Zenject;

namespace Alensia.Core.Navigation
{
    public abstract class Navigator : INavigator, ILateTickable
    {
        public Vector3? Destination { get; set; }

        public Quaternion? TargetRotation { get; set; }

        public bool FaceDestination { get; set; }

        public NavMeshAgent Agent { get; private set; }

        public NavigatorSettings Settings { get; private set; }

        public Transform Transform { get; private set; }

        public Navigator(NavMeshAgent agent, NavigatorSettings settings, Transform transform)
        {
            Assert.IsNotNull(agent);
            Assert.IsNotNull(settings);
            Assert.IsNotNull(transform);

            Agent = agent;
            Settings = settings;
            Transform = transform;
        }

        protected virtual void OnDestinationChange(Vector3? destination)
        {
        }

        protected virtual void OnTargetRotationChange(Quaternion? rotation)
        {
        }

        protected abstract void UpdatePosition();

        public virtual void LateTick()
        {
            UpdatePosition();
        }
    }
}