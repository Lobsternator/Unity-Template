using System;
using UnityEngine;

namespace Template.Physics
{
    /// <summary>
    /// Glossary information for <see cref="ContactInfo"/>.
    /// </summary>
    public enum ContactType
    {
        /// <summary>
        /// Contact involves no triggers.
        /// </summary>
        Collision,
        /// <summary>
        /// Contact involves triggers.
        /// </summary>
        Trigger
    }

    /// <summary>
    /// Stores glossary information about a <see cref="ContactPoint"/> event.
    /// </summary>
    public struct ContactInfo : IEquatable<ContactInfo>
    {
        public Collider Collider { get; }
        public ContactType ContactType { get; }

        public ContactInfo(Collider collider, ContactType contactType)
        {
            Collider    = collider;
            ContactType = contactType;
        }

        public static bool operator ==(ContactInfo contactInfoA, ContactInfo contactInfoB)
        {
            return contactInfoA.Collider == contactInfoB.Collider && contactInfoA.ContactType == contactInfoB.ContactType;
        }
        public static bool operator !=(ContactInfo contactInfoA, ContactInfo contactInfoB)
        {
            return contactInfoA.Collider != contactInfoB.Collider || contactInfoA.ContactType != contactInfoB.ContactType;
        }

        public bool Equals(ContactInfo other)
        {
            return this == other;
        }
        public override bool Equals(object obj)
        {
            if (obj is ContactInfo)
                return Equals((ContactInfo)obj);
            else
                return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return Collider.GetHashCode() ^ ContactType.GetHashCode();
        }
    }

    /// <summary>
    /// Stores glossary information about a <see cref="ContactPoint2D"/> event.
    /// </summary>
    public struct ContactInfo2D : IEquatable<ContactInfo2D>
    {
        public Collider2D Collider { get; }
        public ContactType ContactType { get; }

        public ContactInfo2D(Collider2D collider, ContactType contactType)
        {
            Collider    = collider;
            ContactType = contactType;
        }

        public static bool operator ==(ContactInfo2D contactInfoA, ContactInfo2D contactInfoB)
        {
            return contactInfoA.Collider == contactInfoB.Collider && contactInfoA.ContactType == contactInfoB.ContactType;
        }
        public static bool operator !=(ContactInfo2D contactInfoA, ContactInfo2D contactInfoB)
        {
            return contactInfoA.Collider != contactInfoB.Collider || contactInfoA.ContactType != contactInfoB.ContactType;
        }

        public bool Equals(ContactInfo2D other)
        {
            return this == other;
        }
        public override bool Equals(object obj)
        {
            if (obj is ContactInfo2D)
                return Equals((ContactInfo2D)obj);
            else
                return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return Collider.GetHashCode() ^ ContactType.GetHashCode();
        }
    }
}
