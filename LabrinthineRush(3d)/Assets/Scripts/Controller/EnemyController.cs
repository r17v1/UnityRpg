using UnityEngine.AI;
using UnityEngine;
using RPG.combat;
using RPG.movement;
using RPG.Helper;

namespace RPG.controller
{
    public class EnemyController : MonoBehaviour
    {
        AiMovement move;
        Combat combat;
        Transform player;
        EnemyStats stats;
        [SerializeField] float aggroDistance = 10f;
        [SerializeField] float attackDistance = 2.2f;
        void Start()
        {
   
            
            move = GetComponent<AiMovement>();
            combat = GetComponent<Combat>();
            player = GameObject.Find("Player").transform;
            stats = GetComponent<EnemyStats>();
            
            
        }


        void Update()
        {
            if (stats.death == true) return;
            float distanceFromPlayer = Vector3.Distance(player.transform.position, transform.position);
            if (distanceFromPlayer<= attackDistance)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Rotation.LookAtY(transform.position, player.transform.position),Time.deltaTime*6f);
                combat.Attack();
            }
            if ( distanceFromPlayer< aggroDistance && !combat.IsAttacking())
            {
                GetComponent<NavMeshAgent>().stoppingDistance = attackDistance;
                move.MoveTo(player.position);
            }
            else if(distanceFromPlayer > aggroDistance && !combat.IsAttacking())
            {
                GetComponent<NavMeshAgent>().stoppingDistance = 0;
                move.MoveTo(stats.initialPosition);
            }

        }
    }
}