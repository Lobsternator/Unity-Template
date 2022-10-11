using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Template.Events;

namespace Template.Gameplay
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class TestObjectB2D : MonoBehaviour
    {
        private Rigidbody2D _rigidbody;

        private void OnTest2D(TestObjectA2D objA)
        {
            _rigidbody.AddForce(Vector2.up * objA.LaunchForce, ForceMode2D.Impulse);
        }

        private void OnEnable()
        {
            EventManager.Instance.GameplayEvents.Test2D += OnTest2D;
        }
        private void OnDisable()
        {
            if (EventManager.IsApplicationQuitting)
                return;

            EventManager.Instance.GameplayEvents.Test2D -= OnTest2D;
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }
    }
}
