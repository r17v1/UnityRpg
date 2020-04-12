using System.Collections.Generic;
using System;
using UnityEngine;


namespace RPG.Combat
{
    public class AiCombatController : MonoBehaviour
    {
        public List<EnemyAttacks> attacks;
        public string[] attackNames;
        public float[] attackDamages;
        public float[] attackPoiseDamages;
        public bool[] attackKnockBacks;
        public float[] attackRanges;
        public float[] attackDeltaAngles;
        public int[] attackWeights;
        public float[] attackCooldownTimes;
        public float crossFade;
        float crossFadeCooldown = 0;

        Animator anim;
        void Start()
        {
            anim = GetComponent<Animator>();
            attacks = new List<EnemyAttacks>();
            for (int i = 0; i < attackNames.Length; i++)
            {
                attacks.Add(new EnemyAttacks(attackNames[i], attackDamages[i], attackPoiseDamages[i], 0, 0, attackKnockBacks[i], attackWeights[i], attackRanges[i], attackDeltaAngles[i], attackCooldownTimes[i], crossFade, anim));
            }

        }
        void Update()
        {
            if (crossFadeCooldown > 0) crossFadeCooldown -= Time.deltaTime;
            foreach (var attack in attacks)
            {
                attack.Update(Time.deltaTime);
            }
        }
        public bool WillAttack(float range, float deltaAngle)
        {
            if (anim.GetBool("canMove") == false) return false;
            if (crossFadeCooldown > 0) return false;

            System.Random rand = new System.Random(Environment.TickCount);
            int weightSum = 0;
            List<EnemyAttacks> availableAttacks = new List<EnemyAttacks>();
            foreach (var attack in attacks)
            {
                if (attack.isInCooldown() == false && attack.range >= range && attack.deltaAngle >= deltaAngle)
                {
                    weightSum += attack.weight;
                    availableAttacks.Add(attack);
                }
            }
            if (availableAttacks.Count == 0 || weightSum == 0) return false;

            int randomWeight = rand.Next(0, weightSum);
            weightSum = 0;
            foreach (var attack in availableAttacks)
            {
                weightSum += attack.weight;
                if (randomWeight < weightSum)
                {
                    attack.Play();
                    crossFadeCooldown = crossFade;
                    return true;
                }
            }
            return false;
        }
        public float MaxAttackRange()
        {
            float maxRange = 0f;
            foreach (var attack in attacks)
                maxRange = Mathf.Max(attack.range, maxRange);
            return maxRange;
        }
    }
}