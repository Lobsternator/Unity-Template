using UnityEngine;

namespace Template.Gameplay
{
    /// <summary>
    /// Complimentary test script for <see cref="TestObjectA2D"/>.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class TestObjectB2D : MonoBehaviour
    {
        private Rigidbody2D _rigidbody;

        public void Launch(float force)
        {
            _rigidbody.AddForce(Vector2.up * force, ForceMode2D.Impulse);
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }
    }
}
