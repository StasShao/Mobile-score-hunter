using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigItemPoints : Item
{
    [SerializeField] protected string attachedCollider;
    [SerializeField] protected int ScorePointAdd;
    public override void AddPoints(Collider col)
    {
        pointer.OnPointAdd(col,attachedCollider,ScorePointAdd);
    }

    public override void Begin()
    {
        pointer = new ActionSystems.Pointer(gameObject);
    }
    private void Start()
    {
        Begin();
    }
    private void OnTriggerEnter(Collider other)
    {
        AddPoints(other);
    }
}
