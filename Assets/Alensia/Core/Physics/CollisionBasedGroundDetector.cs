using System.Collections.Generic;
using System.Linq;
using Alensia.Core.Common;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace Alensia.Core.Physics
{
    public class CollisionBasedGroundDetector : GroundDetector
    {
        public override GroundDetectionSettings Settings { get; }

        public override Collider Target => CollisionDetector.Target;

        public ICollisionDetector CollisionDetector { get; set; }

        public CollisionBasedGroundDetector(
            ICollisionDetector detector,
            GroundHitEvent groundHit,
            GroundLeaveEvent groundLeft) : this(
            new GroundDetectionSettings(), detector, groundHit, groundLeft)
        {
        }

        [Inject]
        public CollisionBasedGroundDetector(
            GroundDetectionSettings settings,
            ICollisionDetector detector,
            GroundHitEvent groundHit,
            GroundLeaveEvent groundLeft) : base(groundHit, groundLeft)
        {
            Assert.IsNotNull(settings, "settings != null");
            Assert.IsNotNull(detector, "detector != null");

            Settings = settings;
            CollisionDetector = detector;

            OnInitialize.Subscribe(_ => AfterInitialize()).AddTo(this);
            OnDispose.Subscribe(_ => AfterDispose()).AddTo(this);
        }

        private void AfterInitialize()
        {
            CollisionDetector.CollisionEntered.Listen(OnCollisionEnter);
            CollisionDetector.CollisionExited.Listen(OnCollisionExit);
        }

        private void AfterDispose()
        {
            CollisionDetector.CollisionEntered.Unlisten(OnCollisionEnter);
            CollisionDetector.CollisionExited.Unlisten(OnCollisionExit);
        }

        protected virtual void OnCollisionEnter(Collision collision)
        {
            OnDetectGround(FindGroundContacts(collision.contacts));
        }

        protected virtual void OnCollisionExit(Collision collision)
        {
            OnDetectGround(FindGroundContacts(collision.contacts));
        }

        protected virtual IEnumerable<Collider> FindGroundContacts(
            IEnumerable<ContactPoint> contacts)
        {
            var center = Target.bounds.center.y;
            var extent = Target.bounds.extents.y;

            var bottom = center - extent;

            var grounds = from c in contacts
                where c.point.y <= bottom + Settings.Tolerance && IsGround(c.otherCollider)
                select c.otherCollider;

            return grounds;
        }
    }
}