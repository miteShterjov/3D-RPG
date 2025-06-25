using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction
    {
        [SerializeField] private Transform target;
        [SerializeField] private float maxSpeed = 7f;
        private float speed;
        private NavMeshAgent navMeshAgent;

        public float Speed { get => speed; set => speed = value; }

        private void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        void Update()
        {
            navMeshAgent.enabled = !GetComponent<Health>().IsDead();
            UpdateAnimator();
        }

        // Starts a move action towards the specified destination
        // It uses the ActionScheduler to ensure that the action can be cancelled
        // and that the character can only perform one action at a time
        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination, speedFraction);
        }

        // Moves the character to the specified destination using NavMeshAgent
        // It sets the destination and speed of the NavMeshAgent
        public void MoveTo(Vector3 destination, float speedFraction)
        {
            navMeshAgent.destination = destination;
            navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
            navMeshAgent.isStopped = false;
        }

        // The Cancel method stops the NavMeshAgent from moving
        // This is called when the action is cancelled, such as when the character stops moving
        public void Cancel()
        {
            navMeshAgent.isStopped = true;
        }

        // Updates the animator with the current speed of the character
        // It calculates the speed based on the NavMeshAgent's velocity
        private void UpdateAnimator()
        {
            GetSpeed();
            GetComponent<Animator>().SetFloat("forwardSpeed", Speed);
        }

        // Return the current speed of the character
        // It calculates the speed based on the NavMeshAgent's velocity
        // This is used to update the animator and ensure smooth movement animations
        private float GetSpeed()
        {
            Vector3 velocity = navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            return Speed = localVelocity.z;
        }
    }
}