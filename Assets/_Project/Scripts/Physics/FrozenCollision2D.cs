using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Template.Physics
{
    public class FrozenCollision2D
    {
        public ContactPoint2D[] contactPoints;

        public FrozenCollision2D(Collision2D collision)
        {
            contactPoints = new ContactPoint2D[collision.contactCount];
            collision.GetContacts(contactPoints);
        }
    }
}
