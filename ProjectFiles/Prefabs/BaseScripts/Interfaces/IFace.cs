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