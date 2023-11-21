using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ActionSystems;
public class Sword : EnemyDamageZone
{
    protected Damager damager;
    [SerializeField] private int DamageValue;
    [SerializeField] private string CollideTag;

    public override void Begin()
    {
        damager = new Damager();
    }

    public override void OnCollisionDamage(Collision col)
    {
        damager.OnColissionDamage(col,DamageValue,CollideTag);
    }

    public override void OnTriggerDamage(Collider col)
    {
        
    }
    private void Start()
    {
        Begin();
    }
    private void OnCollisionEnter(Collision collision)
    {
        OnCollisionDamage(collision);
    }
}
