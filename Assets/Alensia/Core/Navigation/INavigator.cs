using Alensia.Core.Common;
using UnityEngine;
using UnityEngine.AI;

namespace Alensia.Core.Navigation
{
    public interface INavigator : ITransformable
    {
        Vector3? Destination { get; set; }

        Quaternion? TargetRotation { get; set; }

        bool FaceDestination { get; set; }

        NavMeshAgent Agent { get; }

        NavigatorSettings Settings { get; }
    }
}