using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] private GameObject weaponPrefab = null;
        [SerializeField] private AnimatorOverrideController animatorOverrideController = null;
        [SerializeField] private Projectile projectile = null;
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float weaponDamage = 5f;
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] private bool isRightHanded = true;

        public float WeaponRange { get => weaponRange; set => weaponRange = value; }
        public float WeaponDamage { get => weaponDamage; set => weaponDamage = value; }
        public float TimeBetweenAttacks { get => timeBetweenAttacks; set => timeBetweenAttacks = value; }


        // Spawns a weapon prefab at the specified hand transform and sets the animator controller if provided.
        // The weapon will be parented to the specified hand transform.
        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            if (weaponPrefab != null)
            {
                Transform handTransform = isRightHanded ? rightHand : leftHand;
                GameObject equipedPrefab = Instantiate(weaponPrefab, handTransform.position, handTransform.rotation);
                equipedPrefab.transform.parent = isRightHanded ? rightHand : leftHand;
            }
            if (animatorOverrideController != null)
            {
                animator.runtimeAnimatorController = animatorOverrideController;
            }
        }

        // Checks if the weapon has a projectile associated with it.
        public bool HasProjectile()
        {
            return projectile != null;
        }

        // Launches a projectile from the specified hand transforms towards the target's health component.
        public void LounchProjectile(Transform rightHand, Transform leftHand, Health target)
        {
            if (projectile == null) return;

            Projectile projectileInstance = Instantiate(projectile,
                isRightHanded ? rightHand.position : leftHand.position,
                Quaternion.identity);
            projectileInstance.SetTarget(target, weaponDamage);
        }
    }
}