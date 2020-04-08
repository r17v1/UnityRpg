using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using  RPG.combat;
public class DamageOnCollision : MonoBehaviour
{
    bool damageCollider = false;

    public Collider sword;

    Animator anim;

    private HashSet<Collider> damage;


    float weaponDamage = 200f;

    private void Start()
    {
        //anim = GetComponent<Animator>();
        damage = new HashSet<Collider>();
}

    private void Update()
    {
        if(damageCollider)
        {
            Collider[] cols = Physics.OverlapBox(sword.transform.position, sword.transform.localScale / 2, sword.transform.rotation, LayerMask.GetMask("hitbox"));
            Debug.Log(cols.Length);


            foreach (var c in cols)
            {

                if (c.transform == transform) continue;
                if (c.transform.GetComponent<Animator>().GetBool("dodging"))
                {
                    Debug.Log("Dodging");
                    continue;
                }

                damage.Add(c);
            }
        }
    }

    void OpenDamageColliders()
    {
        damageCollider = true;
    }
    void CloseDamageColliders()
    {
        damageCollider = false;
        foreach (var d in damage)
        {
            d.transform.GetComponent<Stats>().TakeDamage(weaponDamage);
        }
    }
}
