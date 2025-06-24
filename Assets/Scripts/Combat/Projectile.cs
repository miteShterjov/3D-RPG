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

        void Update()
        {
            if (target == null) return;

            transform.LookAt(GetAimLocation());
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        // This method is called when the projectile collides with something
        // It checks if the collided object has a Health component
        public void SetTarget(Health newTarget)
        {
            this.target = newTarget;
        }

        // This method checks if the projectile is in range of the target
        // It is used to determine if the projectile should be destroyed
        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
            if (targetCapsule == null) return target.transform.position;
            return target.transform.position + Vector3.up * targetCapsule.height / 2f;
        }
    }
}
