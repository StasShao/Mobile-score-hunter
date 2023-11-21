using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ActionSystems;
public abstract class Item : MonoBehaviour
{
    protected Pointer pointer;
    public abstract void Begin();
    public abstract void AddPoints(Collider col);
}
