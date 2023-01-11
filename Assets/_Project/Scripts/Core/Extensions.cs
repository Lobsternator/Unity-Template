using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Template.Core
{
    public static class Extensions
    {
        #region Physics
        public static Vector3 GetAverageContactNormal(this Collision collision)
        {
            ContactPoint[] contactPoints = new ContactPoint[collision.contactCount];
            collision.GetContacts(contactPoints);

            Vector3 averageContactNormal = Vector3.zero;
            foreach (ContactPoint contactPoint in contactPoints)
                averageContactNormal += contactPoint.normal;

            return averageContactNormal.normalized;
        }
        public static Vector2 GetAverageContactNormal(this Collision2D collision)
        {
            ContactPoint2D[] contactPoints = new ContactPoint2D[collision.contactCount];
            collision.GetContacts(contactPoints);

            Vector2 averageContactNormal = Vector2.zero;
            foreach (ContactPoint2D contactPoint in contactPoints)
                averageContactNormal += contactPoint.normal;

            return averageContactNormal.normalized;
        }

        public static Vector3 GetClosestContactNormal(this Collision collision, Vector3 target)
        {
            ContactPoint[] contactPoints = new ContactPoint[collision.contactCount];
            collision.GetContacts(contactPoints);

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
        public static Vector2 GetClosestContactNormal(this Collision2D collision, Vector2 target)
        {
            ContactPoint2D[] contactPoints = new ContactPoint2D[collision.contactCount];
            collision.GetContacts(contactPoints);

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
        #endregion Physics

        #region Collections
        public static int Count<T>(this IList<T> list, Func<T, bool> predicate)
        {
            int count = 0;

            for (int i = 0; i < list.Count; i++)
                if (predicate(list[i]))
                    count++;

            return count;
        }
        #endregion Collections
    }
}
