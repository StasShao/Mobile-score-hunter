using UnityEngine;
public abstract class EnemyDamageZone : MonoBehaviour
{
    public abstract void Begin();
    public abstract void OnCollisionDamage(Collision col);
    public abstract void OnTriggerDamage(Collider col);
}
