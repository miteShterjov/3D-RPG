using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction
    {
        [SerializeField] Transform target;
        [SerializeField] float maxSpeed = 7f;
        private float speed;
        NavMeshAgent navMeshAgent;

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

        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination, speedFraction);
        }

        public void MoveTo(Vector3 destination, float speedFraction)
        {
            navMeshAgent.destination = destination;
            navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
            navMeshAgent.isStopped = false;
        }

        public void Cancel()
        {
            navMeshAgent.isStopped = true;
        }

        private void UpdateAnimator()
        {
            GetSpeed();
            GetComponent<Animator>().SetFloat("forwardSpeed", Speed);
        }

        private float GetSpeed()
        {
            Vector3 velocity = navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            return Speed = localVelocity.z;
        }
    }
}