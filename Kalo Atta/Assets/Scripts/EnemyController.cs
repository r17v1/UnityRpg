using UnityEngine.AI;
using UnityEngine;
using RPG.Combat;
using RPG.Movement;
using RPG.Helper;


namespace RPG.Controller
{
    public class EnemyController : MonoBehaviour
    {
        AiMovement move;
        CombatController combat;
        Transform player;
        EnemyStats stats;
        [SerializeField] float aggroDistance = 10f;
        [SerializeField] float attackDistance = 2.2f;
        Animator anim;

        public float rotationSpeed = 6f;

        public string[] ohAttacks;
        public string[] thAttacks;
        public bool twoHand;

        int noOfCombos;

        bool canMove;

        void Start()
        {

            anim = GetComponent<Animator>();
            move = GetComponent<AiMovement>();
            combat = GetComponent<CombatController>();
            player = GameObject.Find("Player").transform;
            stats = GetComponent<EnemyStats>();

            
            
        }



        void Update()
        {

            canMove= canMove = anim.GetBool("canMove");
            if (stats.death == true) return;
            float distanceFromPlayer = Vector3.Distance(player.transform.position, transform.position);
            if (distanceFromPlayer<= attackDistance)
            {
                if(canMove)
                transform.rotation = Quaternion.Slerp(transform.rotation, Rotation.LookAtY(transform.position, player.transform.position),Time.deltaTime* rotationSpeed);
                Attack();
            }
            if ( distanceFromPlayer< aggroDistance && canMove)
            {
                Debug.Log("if2");
                GetComponent<NavMeshAgent>().stoppingDistance = attackDistance;
                move.MoveTo(player.position);
            }
            else if(distanceFromPlayer > aggroDistance && canMove)
            {
                Debug.Log("if3");
                GetComponent<NavMeshAgent>().stoppingDistance = 0;
                move.MoveTo(stats.initialPosition);
            }

        }

        void Attack()
        {
            noOfCombos = twoHand ? thAttacks.Length : ohAttacks.Length;
            System.Random rnd = new System.Random();
            int currentIndex = rnd.Next(0,noOfCombos);
            //combat.updateVariables(Time.deltaTime, twoHand, ohAttacks, thAttacks, currentIndex, canMove);
            //combat.Tick();
            //combat.Attack();
        }
    }
}