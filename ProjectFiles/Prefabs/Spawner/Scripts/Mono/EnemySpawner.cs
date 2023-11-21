using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PoolSystems;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<Transform> SpawnpositionList;
    protected PoolMono<EnemyCharacter> pool;
    [SerializeField] private EnemyCharacter Prefab;
    [SerializeField] private Transform SpawnContainer;
    [SerializeField] private int SpawnPoolCount;
    [SerializeField] private bool AutoExpand;
    
    private void Start()
    {
        pool = new PoolMono<EnemyCharacter>(Prefab,SpawnPoolCount,SpawnContainer,AutoExpand);
        for (int i = 0; i < SpawnpositionList.Count; i++)
        {
            SpawnpositionList[i].parent = null;
            SpawnContainer.position = SpawnpositionList[i].position;
            pool.GetFreeElement().transform.parent = null;
        }
    }
}
