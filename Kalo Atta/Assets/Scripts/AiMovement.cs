using UnityEngine;
using UnityEngine.AI;
using RPG.Helper;

namespace RPG.Movement
{
    public class AiMovement : MonoBehaviour
    {
        NavMeshAgent agent;
        [SerializeField] private float rotationSpeed = 4f;
        public float runSpeed;
        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
        }
        void Update()
        {
            GetComponent<Animator>().SetFloat("vertical", agent.velocity.magnitude / runSpeed);
        }

        public void MoveTo(Vector3 target)
        {
            agent.isStopped = false;
            agent.SetDestination(target);
        }
        public void SetStoppingDistance(float distance)
        {
            agent.stoppingDistance = distance;
        }
        public void Stop()
        {
            agent.isStopped=true;
        }
        public void SetWalkSpeed()
        {
            agent.speed = runSpeed / 2;
        }
        public void SetRunSpeed()
        {
            agent.speed = runSpeed;
        }
    }
}