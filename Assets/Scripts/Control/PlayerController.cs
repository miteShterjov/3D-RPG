using System;
using RPG.Core;
using RPG.Movement;
using UnityEngine;
using RPG.Combat;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {

        private void Update()
        {
            if (GetComponent<Health>().IsDead()) return;
            if (InteractWithCombat()) return;
            if (InteractWithMovement()) return;
        }

        // Handles combat interaction with the player
        // It checks for combat targets under the mouse cursor
        // If a target is found and the player can attack it, it initiates the attack
        // Returns true if an interaction was handled, false otherwise
        private bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach (RaycastHit hit in hits)
            {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if (target == null) continue;

                if (!GetComponent<Fighter>().CanAttack(target.gameObject))
                {
                    continue;
                }

                if (Input.GetMouseButton(0))
                {
                    GetComponent<Fighter>().Attack(target.gameObject);
                }
                return true;
            }
            return false;
        }

        // Handles movement interaction with the player
        // It checks if the player clicked on a point in the world  
        // If a point is clicked, it starts a move action towards that point
        // Returns true if an interaction was handled, false otherwise
        private bool InteractWithMovement()
        {
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                {
                    GetComponent<Mover>().StartMoveAction(hit.point, 1f);
                }
                return true;
            }
            return false;
        }

        // returns a ray from the camera through the mouse position
        // This is used to detect where the player is clicking in the game world
        // It is essential for both combat and movement interactions
        // It uses the main camera to create a ray that can be used for raycasting
        // This allows the player to interact with the game world by clicking on objects or points
        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}