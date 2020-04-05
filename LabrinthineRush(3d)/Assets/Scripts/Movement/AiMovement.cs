using UnityEngine;
using UnityEngine.AI;
using RPG.Helper;

namespace RPG.movement
{
    public class AiMovement : MonoBehaviour
    {
        public float playerMaxSpeed;
        NavMeshAgent agent;
        [SerializeField] private float rotationSpeed = 4f;
        // Start is called before the first frame update
        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            playerMaxSpeed = 7.0f;

        }

        // Update is called once per frame
        void Update()
        {
            GetComponent<Animator>().SetFloat("forwardSpeed", agent.velocity.magnitude / playerMaxSpeed);
        }

        public void MoveTo(Vector3 target)
        {
            //transform.rotation = Quaternion.Slerp(transform.rotation, Rotation.LookAtY(transform.position, target), rotationSpeed * Time.deltaTime);
            agent.SetDestination(target);

        }


    }
}