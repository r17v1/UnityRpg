using System;
using UnityEngine;

namespace RPG.Combat
{
    public class PlayerAttacks : Attacks
    {
        public float staminaCost;


        public PlayerAttacks(string animationName, float damage, float poiseDamage, float criticalChance, float criticalMultiplier, bool knockBack, float crossFadeTime, Animator anim,float staminaCost)
        {
            this.animationName = animationName;
            this.damage = damage;
            this.poiseDamage = poiseDamage;
            this.criticalChance = criticalChance;
            this.criticalMultiplier = criticalMultiplier;
            this.knockBack = knockBack;
            this.crossFadeTime = crossFadeTime;
            this.anim = anim;
            this.staminaCost = staminaCost;
            rand = new System.Random(Environment.TickCount);
        }


        public override void Play()
        {

            float totalDamage = damage;
            if (rand.NextDouble() < criticalChance)
            {
                damage *= criticalMultiplier;
            }
            anim.SetFloat("damage", totalDamage);
            anim.SetFloat("poise", poiseDamage);
            anim.SetBool("knockBack", knockBack);
            anim.CrossFade(animationName, crossFadeTime);
        }
        
    }
}