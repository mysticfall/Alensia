using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace Alensia.Core.Physics
{
    public abstract class GroundDetector : IGroundDetector, IDisposable
    {
        public abstract GroundDetectionSettings Settings { get; }

        public abstract Collider Target { get; }

        public IEnumerable<Collider> Grounds { get; private set; }

        public bool Grounded
        {
            get { return Grounds.Any(); }
        }

        public GroundHitEvent GroundHit { get; private set; }

        public GroundLeaveEvent GroundLeft { get; private set; }

        protected GroundDetector(GroundHitEvent groundHit, GroundLeaveEvent groundLeft)
        {
            Assert.IsNotNull(groundHit, "groundHit != null");
            Assert.IsNotNull(groundLeft, "groundLeft != null");

            GroundHit = groundHit;
            GroundLeft = groundLeft;

            Grounds = Enumerable.Empty<Collider>();
        }

        public virtual void Dispose()
        {
            Grounds = Enumerable.Empty<Collider>();
        }

        protected virtual bool IsGround(Collider collider)
        {
            return true;
        }

        protected void OnDetectGround(IEnumerable<Collider> grounds)
        {
            var collection = grounds.ToList();

            var oldContacts = Grounds.Except(collection).ToList();
            var contacts = collection.Except(Grounds).ToList();

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