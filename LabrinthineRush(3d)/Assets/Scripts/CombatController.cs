using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Combat
{
    public class CombatController : MonoBehaviour
    {
        public bool twoHand;
        public float crossFadeTime = 0.1f;
        float crossFading = 0;
        string[] currentAttacks;
        string[] ohAttacks;
        string[] thAttacks;
        Animator anim;
        Stats stats;
        float deltaTime;
        public float attackCooldownTime = 0.3f;
        float attackCooldown = 0f;
        public float dodgeCooldownTime = 0.1f;
        public float dodgeCooldown = 0;
        public float thAttackStamina = 20f;
        public float ohAttackStamina = 15f;
        float attackStamina;
        public float dodgeStamina = 25f;
        bool canMove;
        int currentAttackIndex;
        bool lockOn;
        float rollV, rollH;

        private void Start()
        {
            anim = GetComponent<Animator>();
            stats = GetComponent<Stats>();
            crossFading = 0;
        }

        public void Tick()
        {
            if (twoHand)
            {
                currentAttacks = thAttacks;
                attackStamina = thAttackStamina;
            }
            else
            {
                currentAttacks = ohAttacks;
                attackStamina = ohAttackStamina;
            }
            crossFading -= deltaTime;
            if (crossFading < 0) crossFading = 0;

            if (canMove && crossFading<=0)
            {
                attackCooldown -= deltaTime;
                dodgeCooldown -= deltaTime;
            }
            if (attackCooldown < 0) attackCooldown = 0;
            if (dodgeCooldown < 0) dodgeCooldown = 0;

        }

        public void updateVariables(float inputDeltaTime, bool inputTwoHand, string[] inputOhAttacks, string[] inputThAttacks, int inputCurrentAttackIndex, bool inputCanMove)
        {
            deltaTime = inputDeltaTime;
            twoHand = inputTwoHand;
            ohAttacks = inputOhAttacks;
            thAttacks = inputThAttacks;
            currentAttackIndex = inputCurrentAttackIndex;
            canMove = inputCanMove;
        }

        public void setDodgeValirable(float inputRollV, float inputRollH)
        {
            rollV = inputRollV;
            rollH = inputRollH;
        }

        public void Attack()
        {

            
            if (canMove == false || crossFading > 0)
            { 
                if (stats.currentStamina > 0f)
                {
                    if (anim.GetBool("nextAttackChain")) return; 
                    anim.SetBool("nextAttackChain", true);
                    stats.reduceStamina(attackStamina);
                }
                return;
            }
            anim.SetBool("nextAttackChain", false);
            if (attackCooldown<=0f && stats.currentStamina >0f)
            {
                anim.CrossFade(currentAttacks[currentAttackIndex], crossFadeTime);
                crossFading = crossFadeTime;
                attackCooldown = attackCooldownTime;
                stats.reduceStamina(attackStamina);
            }

        }
    
        public void Dodge()
        {
            anim.SetFloat("rollV", rollV);
            anim.SetFloat("rollH", rollH);

            if (canMove == false|| crossFading>0)
            {
                if (stats.currentStamina > 0f)
                {
                    if (anim.GetBool("dodgeChain")) return;
                    anim.SetBool("dodgeChain", true);
                    stats.reduceStamina(dodgeStamina);
                }
                return;
            }
            anim.SetBool("dodgeChain", false);
            if (dodgeCooldown <= 0f && stats.currentStamina>0f)
            {
                anim.CrossFade("dodge", crossFadeTime);
                crossFading = crossFadeTime;
                dodgeCooldown = dodgeCooldownTime;
                stats.reduceStamina(dodgeStamina);
            }
        }
    }
}