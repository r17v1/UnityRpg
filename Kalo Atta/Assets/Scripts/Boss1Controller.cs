using System;
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
        AiCombatController combat;
        public Transform player;
        EnemyStats stats;
        [SerializeField] float aggroDistance = 30f;
        Animator anim;
        public float rotationSpeed = 6f;
        bool canMove;
        public float minCooldown = 0.3f, maxCooldown = 1f;
        float coolDown;
        float maxAttackRange;
        float distanceFromPlayer;
        float angleFromPlayer;
        System.Random rand;

        void Start()
        {

            rand = new System.Random(Environment.TickCount);
            anim = GetComponent<Animator>();
            move = GetComponent<AiMovement>();
            combat = GetComponent<AiCombatController>();
            player = GameObject.Find("Player").transform;
            stats = GetComponent<EnemyStats>();
            health.setMaxHealth(GetComponent<Stats>().health);
           
            coolDown = 0;
        }



        void Update()
        {

            maxAttackRange = combat.MaxAttackRange();
            health.setHealth(GetComponent<Stats>().currentHealth);
            canMove = canMove = anim.GetBool("canMove");
            if (stats.death == true) return;
            distanceFromPlayer = DiatanceFromPlayer();
            angleFromPlayer = DelAngle();
            if (coolDown > 0 && canMove) coolDown -= Time.deltaTime;

            
            if (canMove) PerformAction();

        }

        float DelAngle()
        {
            Vector3 lookDirection = transform.forward;
            Vector3 playerRelativePosition = (player.transform.position - transform.position).normalized;
            float angle = Vector3.Angle(lookDirection, playerRelativePosition);
            return Mathf.Abs(angle);
        }
        float DiatanceFromPlayer()
        {
            return Vector3.Distance(player.transform.position, transform.position);
        }

        void PerformAction()
        {
            if (distanceFromPlayer < aggroDistance)
            {
                
                bool actionPerformed;
                transform.rotation = Quaternion.Slerp(transform.rotation, Rotation.LookAtY(transform.position, player.position),rotationSpeed*Time.deltaTime);

                if (distanceFromPlayer > maxAttackRange || coolDown>0)
                {
                   // Debug.Log("DP"+(distanceFromPlayer > maxAttackRange));
                    //Debug.Log(coolDown > 0);
                    move.SetStoppingDistance(maxAttackRange);
                    move.MoveTo(player.position);
                    actionPerformed = true;
                }
                else
                {
                    move.Stop();
                   // Debug.Log("else");
                    actionPerformed = combat.WillAttack(distanceFromPlayer, angleFromPlayer);
                    if(actionPerformed)
                        coolDown = rand.Next(Convert.ToInt32(minCooldown * 10), Convert.ToInt32(maxCooldown * 10 + 1)) / 10f;
                }
               
            }
            else
            {
                move.SetStoppingDistance(0f);
                move.MoveTo(stats.initialPosition);
            }
        }

    }
}
