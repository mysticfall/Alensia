using System.Collections.Generic;
using System.Linq;
using Alensia.Core.Common;
using ModestTree;
using UnityEngine;
using Assert = UnityEngine.Assertions.Assert;

namespace Alensia.Core.Physics
{
    public abstract class GroundDetector : BaseObject, IGroundDetector
    {
        public abstract GroundDetectionSettings Settings { get; }

        public abstract Collider Target { get; }

        public ISet<Collider> Grounds { get; private set; } = new HashSet<Collider>();

        public bool Grounded => Grounds.Any();

        public GroundHitEvent GroundHit { get; }

        public GroundLeaveEvent GroundLeft { get; }

        protected GroundDetector(GroundHitEvent groundHit, GroundLeaveEvent groundLeft)
        {
            Assert.IsNotNull(groundHit, "groundHit != null");
            Assert.IsNotNull(groundLeft, "groundLeft != null");

            GroundHit = groundHit;
            GroundLeft = groundLeft;
        }

        protected virtual bool IsGround(Collider collider) => true;

        protected void OnDetectGround(IEnumerable<Collider> grounds)
        {
            var collection = grounds.ToHashSet();

            var oldContacts = Grounds.Except(collection).ToHashSet();
            var contacts = collection.Except(Grounds).ToHashSet();

            Grounds = collection;

            if (oldContacts.Any())
            {
                GroundLeft.Fire(oldContacts);
            }

            if (contacts.Any())
            {
                GroundHit.Fire(contacts);
            }
        }
    }
}