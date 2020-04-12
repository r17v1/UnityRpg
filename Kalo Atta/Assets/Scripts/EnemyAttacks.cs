
using UnityEngine;
using System;

namespace RPG.Combat
{
    public class EnemyAttacks : Attacks
    {
        float coolDown;
        float currentCooldownTime;

        public int weight;
        public float range;
        public float deltaAngle;
        bool canMove;

        
        public EnemyAttacks(string animationName, float damage,float poiseDamage,float criticalChance, float criticalMultiplier, bool knockBack, int weight, float range, float deltaAngle, float coolDown,float crossFadeTime, Animator anim)
        {
            this.animationName = animationName;
            this.damage = damage;
            this.poiseDamage = poiseDamage;
            this.criticalChance = criticalChance;
            this.criticalMultiplier = criticalMultiplier;
            this.knockBack = knockBack;
            this.weight = weight;
            this.range = range;
            this.deltaAngle = deltaAngle;
            this.coolDown = coolDown;
            this.crossFadeTime = crossFadeTime;
            this.anim = anim;
            rand = new System.Random(Environment.TickCount);
        }

        public override void Play()
        {
            if(canMove && currentCooldownTime<=0)
            {
                float totalDamage = damage;
                if(rand.NextDouble()<criticalChance)
                {
                    damage *= criticalMultiplier;
                }
                anim.SetFloat("damage", totalDamage);
                anim.SetFloat("poise", poiseDamage);
                anim.SetBool("knockBack", knockBack);
                anim.CrossFade(animationName, crossFadeTime);
                currentCooldownTime = coolDown;
            }
        }
        public void Update(float deltaTime)
        {
           canMove = anim.GetBool("canMove");
            if (currentCooldownTime > 0 && canMove ) currentCooldownTime -= deltaTime;
        }
        public bool isInCooldown()
        {
            if (currentCooldownTime <= 0) return false;
            return true;
        }
    }
}