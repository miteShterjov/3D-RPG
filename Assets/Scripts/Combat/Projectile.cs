using System;
using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        // This script is responsible for the projectile behavior
        [SerializeField] private float speed = 5f;
        // This is the target that the projectile will follow
        private Health target = null;
        float damage = 0f;

        void Update()
        {
            if (target == null) return;

            transform.LookAt(GetAimLocation());
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        // This method is called when the projectile collides with something
        // It checks if the collided object has a Health component
        public void SetTarget(Health newTarget, float damage)
        {
            this.target = newTarget;
            this.damage = damage;
        }

        // This method checks if the projectile is in range of the target
        // It is used to determine if the projectile should be destroyed
        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
            if (targetCapsule == null) return target.transform.position;
            return target.transform.position + Vector3.up * targetCapsule.height / 2f;
        }
        void OnTriggerEnter(Collider other)
        {
            // if target has a Health component, apply damage
            if (other.GetComponent<Health>() != target) return;
            target.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
