using UnityEngine;

namespace Template.Gameplay
{
    /// <summary>
    /// Complimentary test script for <see cref="TestObjectA"/>.
    /// </summary>
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    public class TestObjectB : MonoBehaviour
    {
        private Rigidbody _rigidbody;

        public void Launch(float force)
        {
            _rigidbody.AddForce(Vector3.up * force, ForceMode.Impulse);
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }
    }
}
