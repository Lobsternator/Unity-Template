using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Template.Events;

namespace Template.Gameplay
{
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    public class TestObjectB : MonoBehaviour
    {
        private Rigidbody _rigidbody;

        private void OnTest(TestObjectA objA)
        {
            _rigidbody.AddForce(Vector3.up * objA.LaunchForce, ForceMode.Impulse);
        }

        private void OnEnable()
        {
            EventManager.Instance.GameplayEvents.Test += OnTest;
        }
        private void OnDisable()
        {
            if (EventManager.IsApplicationQuitting)
                return;

            EventManager.Instance.GameplayEvents.Test -= OnTest;
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }
    }
}
