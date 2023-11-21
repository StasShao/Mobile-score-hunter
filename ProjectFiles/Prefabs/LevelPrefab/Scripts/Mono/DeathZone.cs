using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ActionSystems;
public class DeathZone : EnemyDamageZone
{
    protected Damager damager = new Damager();
    [SerializeField] protected int Damage;
    [SerializeField] protected string AttachetColliderTag;
    public override void Begin()
    {
       
    }

    public override void OnCollisionDamage(Collision col)
    {
       
    }

    public override void OnTriggerDamage(Collider col)
    {
        damager.OnTriggerDamage(col,Damage,AttachetColliderTag);
    }

    private void OnTriggerEnter(Collider other)
    {
        OnTriggerDamage(other);
    }
}
