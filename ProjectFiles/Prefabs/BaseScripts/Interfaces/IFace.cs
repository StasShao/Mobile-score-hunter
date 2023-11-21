using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IControllable
{
    Vector3 DirectionPoint { get; }
    void SetDirectionPoint(Vector3 point);
}
public interface IAIControllable
{
    Transform EnemyTransform { get; }
    bool IsEnemyDetected { get; }
    void SetEnemyDetected(bool isDetected);
    void SetEnemy(Transform enemy);
}
public interface IHealth
{
    int Health { get; }
    void SetHealth(int health);
   
}
public interface IDamage:IHealth
{
    void Damage(int damage);
}
public interface IPoints
{ 
    int PointsCount { get; }
    void SetPoints(int point);

}