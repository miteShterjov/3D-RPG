using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickUp : MonoBehaviour
    {
        [SerializeField] private Weapon weapon = null;

        // This method is called when another collider enters the trigger 
        // collider attached to the GameObject this script is attached to.
        // If the collider belongs to the player, it equips the weapon and destroys the pickup object.
        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                other.GetComponent<Fighter>().EquipWeapon(weapon);
                Destroy(gameObject);
            }
        }
    }
}
