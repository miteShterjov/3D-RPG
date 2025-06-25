using UnityEngine;

namespace RPG.Core
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private float healthPoints = 100f;
        private bool isDead = false;
        
        public bool IsDead() => isDead;

        // The TakeDamage method reduces the health points by the specified damage amount
        // If health points drop to zero or below, it triggers the Die method
        // This method is used to handle damage taken by the character
        // It ensures that health points do not go below zero
        public void TakeDamage(float damage)
        {
            healthPoints = Mathf.Max(healthPoints - damage, 0);
            if (healthPoints == 0)
            {
                Die();
            }
        }

        // Triggers the death anim of the character
        // Sets isDead to true to prevent further actions
        // This method is called when health points reach zero
        private void Die()
        {
            if (isDead) return;

            isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
    }
}