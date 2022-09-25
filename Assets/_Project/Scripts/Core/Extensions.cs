using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Template.Physics;

namespace Template.Core
{
    public static class Extensions
    {
        #region Physics
        private static Vector3 GetAverageContactNormal_Internal(ContactPoint[] contactPoints)
        {
            Vector3 averageContactNormal = Vector3.zero;
            foreach (ContactPoint contactPoint in contactPoints)
                averageContactNormal += contactPoint.normal;

            return averageContactNormal.normalized;
        }
        private static Vector2 GetAverageContactNormal_Internal(ContactPoint2D[] contactPoints)
        {
            Vector2 averageContactNormal = Vector2.zero;
            foreach (ContactPoint2D contactPoint in contactPoints)
                averageContactNormal += contactPoint.normal;

            return averageContactNormal.normalized;
        }
        private static Vector3 GetClosestContactNormal_internal(ContactPoint[] contactPoints, Vector3 target)
        {
            Vector3 closestContactNormal = Vector3.zero;
            float closest                = float.MinValue;
            foreach (ContactPoint contactPoint in contactPoints)
            {
                float closeness = Vector3.Dot(target, contactPoint.normal);
                if (closeness > closest)
                {
                    closest              = closeness;
                    closestContactNormal = contactPoint.normal;
                }
            }

            return closestContactNormal;
        }
        private static Vector2 GetClosestContactNormal_internal(ContactPoint2D[] contactPoints, Vector2 target)
        {
            Vector2 closestContactNormal = Vector2.zero;
            float closest                = float.MinValue;
            foreach (ContactPoint2D contactPoint in contactPoints)
            {
                float closeness = Vector2.Dot(target, contactPoint.normal);
                if (closeness > closest)
                {
                    closest              = closeness;
                    closestContactNormal = contactPoint.normal;
                }
            }

            return closestContactNormal;
        }

        public static Vector3 GetAverageContactNormal(this Collision collision)
        {
            ContactPoint[] contactPoints = new ContactPoint[collision.contactCount];
            collision.GetContacts(contactPoints);

            return GetAverageContactNormal_Internal(contactPoints);
        }
        public static Vector2 GetAverageContactNormal(this Collision2D collision)
        {
            ContactPoint2D[] contactPoints = new ContactPoint2D[collision.contactCount];
            collision.GetContacts(contactPoints);

            return GetAverageContactNormal_Internal(contactPoints);
        }
        public static Vector3 GetAverageContactNormal(this FrozenCollision frozenCollision)
        {
            return GetAverageContactNormal_Internal(frozenCollision.contactPoints);
        }
        public static Vector2 GetAverageContactNormal(this FrozenCollision2D frozenCollision)
        {
            return GetAverageContactNormal_Internal(frozenCollision.contactPoints);
        }

        public static Vector3 GetClosestContactNormal(this Collision collision, Vector3 target)
        {
            ContactPoint[] contactPoints = new ContactPoint[collision.contactCount];
            collision.GetContacts(contactPoints);

            return GetClosestContactNormal_internal(contactPoints, target);
        }
        public static Vector2 GetClosestContactNormal(this Collision2D collision, Vector2 target)
        {
            ContactPoint2D[] contactPoints = new ContactPoint2D[collision.contactCount];
            collision.GetContacts(contactPoints);

            return GetClosestContactNormal_internal(contactPoints, target);
        }
        public static Vector3 GetClosestContactNormal(this FrozenCollision frozenCollision, Vector3 target)
        {
            return GetClosestContactNormal_internal(frozenCollision.contactPoints, target);
        }
        public static Vector2 GetClosestContactNormal(this FrozenCollision2D frozenCollision, Vector2 target)
        {
            return GetClosestContactNormal_internal(frozenCollision.contactPoints, target);
        }
        #endregion Physics
    }
}
