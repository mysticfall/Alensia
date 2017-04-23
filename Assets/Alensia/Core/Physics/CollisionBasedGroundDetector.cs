using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace Alensia.Core.Physics
{
    public class CollisionBasedGroundDetector : GroundDetector, IInitializable
    {
        public override GroundDetectionSettings Settings
        {
            get { return _settings; }
        }

        public override Collider Target
        {
            get { return CollisionDetector.Target; }
        }

        public ICollisionDetector CollisionDetector { get; set; }

        private readonly GroundDetectionSettings _settings;

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

            _settings = settings;
            CollisionDetector = detector;
        }

        public virtual void Initialize()
        {
            CollisionDetector.CollisionEntered.Listen(OnCollisionEnter);
            CollisionDetector.CollisionExited.Listen(OnCollisionExit);
        }

        public override void Dispose()
        {
            base.Dispose();

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