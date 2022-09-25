using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Template.Physics
{
    public class FrozenCollision
    {
        public ContactPoint[] contactPoints;

        public FrozenCollision(Collision collision)
        {
            contactPoints = new ContactPoint[collision.contactCount];
            collision.GetContacts(contactPoints);
        }
    }
}
