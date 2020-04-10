using UnityEngine.AI;
using UnityEngine;
using RPG.Combat;
using RPG.Movement;
using RPG.Helper;
using RPG.UI;

namespace RPG.Controller
{
    public class Boss1Controller : MonoBehaviour
    {
        public HealthBar health;
        AiMovement move;
        CombatController combat;
        public Transform player;
        EnemyStats stats;
        [SerializeField] float aggroDistance = 10f;
        [SerializeField] float shortAttackDistance = 3f;
        [SerializeField] float longAttackDistance = 6f;
        Animator anim;


        public float rotationSpeed = 6f;

        public string[] shortAttacks;
        public string[] longAttacks;
        public bool longAttack;

        int noOfCombos;

        bool canMove;

        void Start()
        {

            anim = GetComponent<Animator>();
            move = GetComponent<AiMovement>();
            combat = GetComponent<CombatController>();
            player = GameObject.Find("Player").transform;
            stats = GetComponent<EnemyStats>();
            health.setMaxHealth(GetComponent<Stats>().health);
        }



        void Update()
        {
            
            health.setHealth(GetComponent<Stats>().currentHealth);
            canMove = canMove = anim.GetBool("canMove");
            if (stats.death == true) return;
            float distanceFromPlayer = Vector3.Distance(player.transform.position, transform.position);
            if (distanceFromPlayer <= shortAttackDistance)
            {
                longAttack = false;
                if (canMove)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, Rotation.LookAtY(transform.position, player.transform.position), Time.deltaTime * rotationSpeed);

                    Attack();
                }       
            }
            else if(distanceFromPlayer <= longAttackDistance)
            {
                longAttack = true;
                if (canMove)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, Rotation.LookAtY(transform.position, player.transform.position), Time.deltaTime * rotationSpeed);

                    Attack();
                }
            }
            else if (distanceFromPlayer <= aggroDistance && canMove)
            {
                GetComponent<NavMeshAgent>().stoppingDistance = longAttackDistance;
                move.MoveTo(player.position);

                //GetComponent<CharacterController>().Move((player.position - transform.position).normalized * 5*Time.deltaTime);


            }
            else if (distanceFromPlayer > aggroDistance && canMove)
            {
                GetComponent<NavMeshAgent>().stoppingDistance = 0;
                move.MoveTo(stats.initialPosition);
            }

        }

        void Attack()
        {
            noOfCombos = longAttack ? longAttacks.Length : shortAttacks.Length;
            System.Random rnd = new System.Random();
            int currentIndex = rnd.Next(0, noOfCombos);
            combat.updateVariables(Time.deltaTime, longAttack, shortAttacks, longAttacks, currentIndex, canMove);
            combat.Tick();
            combat.Attack();
        }
    }
}
