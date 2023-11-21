using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterMechanicSystems;
public abstract class Character : MonoBehaviour
{
    protected Controller controller;
    public abstract void Begin();
    public abstract void Tick();
    public abstract void FixedTick();
    public abstract void OnPointMove();
    public void Init(Transform characterTransform)
    {
        controller = new Controller(characterTransform);
    }
    
}
