using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        // This is the transform where the weapon will be attached
        [SerializeField] private Transform rightHandTransform = null;
        // This is the transform where the shield/bow will be attached
        [SerializeField] private Transform leftHandTransform = null;
        // Default weapon to equip at the start
        [SerializeField] private Weapon deafultWeapon = null;

        // The target we are attacking, this is a reference to the Health component of the target
        private Health target;
        // The weapon currently equipped by the fighter
        private Weapon currentWeapon = null;
        // Time since the last attack was made, initialized to infinity so that the first attack can be made immediately
        private float timeSinceLastAttack = Mathf.Infinity;

        private void Start()
        {
            target = null;
            EquipWeapon(deafultWeapon);
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (target == null) return;
            if (target.IsDead()) return;

            if (!GetIsInRange())
            {
                GetComponent<Mover>().MoveTo(target.transform.position, 1f);
            }
            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehaviour();
            }
        }

        // This method is called when the fighter hits the target
        // It checks if the target is not null and if the weapon has a projectile
        // If it does, it launches the projectile, otherwise it deals damage directly to the target
        // It is an animation event, meaning it is called from the animation at the right time
        public void Hit()
        {
            if (target == null) { return; }
            if (currentWeapon.HasProjectile())
            {
                currentWeapon.LounchProjectile(rightHandTransform, leftHandTransform, target);
            }
            else
            {
                target.TakeDamage(currentWeapon.WeaponDamage);
            }
            print("Attacking " + target.name);
        }

        // This method checks if the fighter can attack a given target
        // It checks if the target is not null, if it has a Health component, and if the target is not dead
        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) { return false; }
            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
        }

        // This method equips a weapon to the fighter
        // It sets the current weapon to the given weapon and spawns it in the right hand or left hand transforms
        public void EquipWeapon(Weapon weapon)
        {
            currentWeapon = weapon;
            Animator animator = GetComponent<Animator>();
            weapon?.Spawn(rightHandTransform, leftHandTransform, animator);
        }

        // This method is called when the fighter wants to attack a target
        // It starts the action scheduler, sets the target to the Health component of the combat target,
        // and checks if the target is valid for attacking
        // If the target is valid, it will start the attack behaviour
        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        // This method is called when the fighter wants to cancel the attack
        // It stops the attack behaviour and sets the target to null
        public void Cancel()
        {
            StopAttack();
            target = null;
        }

        // This method is called when the fighter is in range of the target and ready to attack
        // It checks if enough time has passed since the last attack and triggers the attack animation
        private void AttackBehaviour()
        {
            transform.LookAt(target.transform);
            if (timeSinceLastAttack > currentWeapon.TimeBetweenAttacks)
            {
                // This will trigger the Hit() event.
                TriggerAttack();
                timeSinceLastAttack = 0;
            }
        }

        // This method triggers the attack animation
        // It resets the stopAttack trigger and sets the attack trigger
        private void TriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger("stopAttack");
            GetComponent<Animator>().SetTrigger("attack");
        }

        // This method stops the attack animation
        // It resets the attack trigger and sets the stopAttack trigger
        private void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("stopAttack");
        }

        // This method checks if the fighter is in range of the target
        // It calculates the distance between the fighter and the target and compares it to the weapon's range
        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < currentWeapon.WeaponRange;
        }

        // This method is called when the fighter wants to shoot a projectile
        // It simply calls the Hit method, which will handle the projectile launch if applicable
        private void Shoot() => Hit();
    }
}