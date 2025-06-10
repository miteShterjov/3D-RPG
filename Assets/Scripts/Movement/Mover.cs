using System;
using RPG.Combat;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour
    {
        static string PLAYER_ANIM_PARAM_SPEED = "forwardSpeed";
        private NavMeshAgent navMeshAgent;
        private Animator animator;
        private Ray lastRay;

        void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            if (navMeshAgent == null) Debug.LogError("NavMeshAgent component is missing on " + gameObject.name);
            animator = GetComponentInChildren<Animator>();
            if (animator == null) Debug.LogError("Animator component is missing on " + gameObject.name);
        }

        void Update()
        {
            // Update the animator with the current speed of the NavMeshAgent.
            // This method converts the world velocity to local space and sets the forward speed parameter.
            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 destination)
        {
            // Moves the player to the specified destination.
            // This method sets the NavMeshAgent's destination to the specified point.
            GetComponent<Fighter>()?.CancelAttack(); // Cancel any ongoing attack before moving
            MoveTo(destination);
        }


        // Moves the player to the specified destination.
        // This method sets the NavMeshAgent's destination to the specified point.
        public void MoveTo(Vector3 destination)
        {
            navMeshAgent.destination = destination; // Set the NavMeshAgent's destination to the hit point);
            navMeshAgent.isStopped = false; // Ensure the NavMeshAgent is not stopped
            Debug.Log($"Moving to {destination}");
        }

        // Stops the movement of the player.
        public void Stop()
        {
            navMeshAgent.isStopped = true; // Stop the NavMeshAgent
            Debug.Log("Movement stopped.");
        }

        // Updates the animator with the current speed of the NavMeshAgent.
        // This method converts the world velocity to local space and sets the forward speed parameter.
        private void UpdateAnimator()
        {
            Vector3 velocity = navMeshAgent.velocity; // Get the current velocity of the NavMeshAgent
            Vector3 localVelocity = transform.InverseTransformDirection(velocity); // Convert world velocity to local velocity
            float speed = localVelocity.z; // Use the z component for forward speed
            animator.SetFloat(PLAYER_ANIM_PARAM_SPEED, speed); // Set the forward speed parameter in the animator
        }
    }
}