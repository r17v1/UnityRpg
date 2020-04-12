
using UnityEngine;
using System;

namespace RPG.Combat
{
    public abstract class Attacks 
    {
        protected string animationName;
        protected float damage;
        protected float poiseDamage;
        protected float criticalChance;
        protected float criticalMultiplier;
        protected bool knockBack;
        protected float crossFadeTime;
        protected Animator anim;

        protected System.Random rand;

        public abstract void Play();
    }
}