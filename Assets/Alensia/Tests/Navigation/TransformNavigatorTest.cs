using System.Collections;
using System.Diagnostics;
using Alensia.Core.Navigation;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TestTools;
using Debug = UnityEngine.Debug;
using TestRange = NUnit.Framework.RangeAttribute;

namespace Alensia.Tests.Navigation
{
    [TestFixture, Description("Test suite for TransformNavigator class.")]
    public class TransformNavigatorTest : TestBase
    {
        public TransformNavigator Navigator;

        [SetUp]
        public void Setup()
        {
            var gameObject = new GameObject();

            var transform = gameObject.GetComponent<Transform>();
            var agent = gameObject.AddComponent<NavMeshAgent>();

            var settings = new NavigatorSettings
            {
                ForwardSpeed = 10,
                BackwardSpeed = 5,
                SidewaysSpeed = 5
            };

            Navigator = new TransformNavigator(agent, settings, transform);
        }

        [UnityTest, Description("It should move towards the target destination.")]
        public IEnumerator ShouldMoveTowardsTargetDestination(
            [TestRange(0, 360, 45)] float heading,
            [Values(1, 10)] float distance)
        {
            Debug.LogFormat("Using data: heading={0}deg., distance={1}m.", heading, distance);

            const float timeout = 3000;

            var transform = Navigator.Transform;
            var destination = Quaternion.AngleAxis(heading, transform.up) * transform.forward * distance;

            Navigator.Destination = destination;

            var clock = Stopwatch.StartNew();

            while (transform.position != destination && clock.ElapsedMilliseconds < timeout)
            {
                Navigator.LateTick();

                yield return null;
            }

            Expect(
                transform.position,
                Is.EqualTo(destination),
                "Navigator has failed to reach the destination.");
        }

        [UnityTest, Description("It should move at a designated speed.")]
        public IEnumerator ShouldMoveAtDesignatedSpeed(
            [Values(0)] float heading,
            [Values(2, 5, 10)] float speed)
        {
            const float distance = 10;
            const float timeout = 6000;
            const float tolerance = 100f;

            Debug.LogFormat("Using data: heading={0}deg., speed={1}m/s.", heading, speed);

            var transform = Navigator.Transform;
            var destination = Quaternion.AngleAxis(heading, transform.up) * transform.forward * distance;

            Navigator.Settings.ForwardSpeed = speed;
            Navigator.Destination = destination;

            var clock = Stopwatch.StartNew();

            while (transform.position != destination && clock.ElapsedMilliseconds < timeout)
            {
                Navigator.LateTick();

                yield return null;
            }

            Assert.IsTrue(transform.position == destination);

            var expectedTime = distance / speed * 1000;

            Expect(
                clock.ElapsedMilliseconds,
                Is.EqualTo(expectedTime).Within(tolerance),
                "Navigator has failed to reach the destination in time.");
        }
    }
}