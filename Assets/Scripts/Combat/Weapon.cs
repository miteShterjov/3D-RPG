using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] private GameObject weaponPrefab = null;
        [SerializeField] private AnimatorOverrideController animatorOverrideController = null;
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float weaponDamage = 5f;
        [SerializeField] float timeBetweenAttacks = 1f;

        public float WeaponRange { get => weaponRange; set => weaponRange = value; }
        public float WeaponDamage { get => weaponDamage; set => weaponDamage = value; }
        public float TimeBetweenAttacks { get => timeBetweenAttacks; set => timeBetweenAttacks = value; }

        public void Spawn(Transform handTransform, Animator animator)
        {
            if (weaponPrefab != null)
            {
                GameObject equipedPrefab = Instantiate(weaponPrefab, handTransform.position, handTransform.rotation);
                equipedPrefab.transform.SetParent(handTransform);
            }
            if (animatorOverrideController != null)
            {
                animator.runtimeAnimatorController = animatorOverrideController;
            }
        }
    }
}