using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterMechanicSystems;
public abstract class Character : MonoBehaviour,IControllable
{
    protected Controller controller;

    public Vector3 DirectionPoint { get; private set; }
    public void SetDirectionPoint(Vector3 point)
    {
        DirectionPoint = point;
    }
    public abstract void Begin();
    public abstract void Tick();
    public abstract void FixedTick();
    public abstract void OnPointMove();
    public void Init(Transform characterTransform)
    {
        controller = new Controller(characterTransform);
    }

    
}
