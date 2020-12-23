using System;
using UnityEngine;
using RPG.Combat;
using RPG.Movement;
using RPG.Helper;


namespace RPG.Controller
{
    public class EnemyController : MonoBehaviour
    {
        AiMovement move;
        AiCombatController combat;
        public Transform player;
        EnemyStats stats;
        public float aggroDistance = 30f;
        public float aggroDistanceBehind = 10f;
        public Transform targetPosition;
        Animator anim;
        public float viewAngle=110f;
        public float rotationSpeed = 6f;
        bool canMove;
        public float minCooldown = 0.3f, maxCooldown = 1f;
        float coolDown;
        float maxAttackRange;
        float distanceFromPlayer;
        float angleFromPlayer;
        System.Random rand;
        bool canSee;
        Vector3 currentDestination;

        void Start()
        {

            rand = new System.Random(Environment.TickCount);
            anim = GetComponent<Animator>();
            move = GetComponent<AiMovement>();
            combat = GetComponent<AiCombatController>();
            player = GameObject.Find("Player").transform;
            stats = GetComponent<EnemyStats>();

            if (targetPosition == null)
                currentDestination = stats.initialPosition;
            else currentDestination = targetPosition.position;

                coolDown = 0;
        }



        void Update()
        {

            maxAttackRange = combat.MaxAttackRange();
            canMove = canMove = anim.GetBool("canMove");
            if (stats.death == true) return;
            distanceFromPlayer = DiatanceFromPlayer();
            angleFromPlayer = DelAngle();
            CheckCanSee();
             
            if (coolDown > 0 && canMove) coolDown -= Time.deltaTime;

            if(targetPosition!=null)
            {
                if (Vector3.Distance(stats.initialPosition, transform.position) < 1)
                    currentDestination = targetPosition.position;
                else if(Vector3.Distance(targetPosition.position, transform.position) < 1) currentDestination = stats.initialPosition;
            }
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

        void CheckCanSee()
        {
            canSee = false;
            Vector3 origin = transform.position;
      
            Vector3 direction = player.position - origin;
            origin.y += .5f;
            direction.y += .5f;

            float distance = aggroDistance;
            RaycastHit hit;
            if(Physics.Raycast(origin,direction,out hit,distance))
            {
                Debug.Log(hit.transform.name);
                if (hit.transform == player)
                    canSee = true;
            }
        }


        void PerformAction()
        {
            if (((distanceFromPlayer < aggroDistance && angleFromPlayer<viewAngle)|| distanceFromPlayer<aggroDistanceBehind) && canSee)
            {

                move.SetRunSpeed();
                bool actionPerformed;
                transform.rotation = Quaternion.Slerp(transform.rotation, Rotation.LookAtY(transform.position, player.position), rotationSpeed * Time.deltaTime);

                if (distanceFromPlayer > maxAttackRange || coolDown > 0)
                {
                    move.SetStoppingDistance(maxAttackRange);
                    move.MoveTo(player.position);
                }
                else
                {
                    move.Stop();
                    // Debug.Log("else");
                    actionPerformed = combat.WillAttack(distanceFromPlayer, angleFromPlayer);
                    if (actionPerformed)
                        coolDown = rand.Next(Convert.ToInt32(minCooldown * 10), Convert.ToInt32(maxCooldown * 10 + 1)) / 10f;
                }

            }
            else
            {
                move.SetWalkSpeed();
                move.SetStoppingDistance(0f);
                move.MoveTo(currentDestination);
            }
        }
    }
}