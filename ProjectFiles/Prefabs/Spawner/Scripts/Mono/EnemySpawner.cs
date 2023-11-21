using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PoolSystems;
public class EnemySpawner : MonoBehaviour
{
    protected PoolMono<EnemyCharacter> pool;
    [SerializeField] private EnemyCharacter Prefab;
    [SerializeField] private Transform SpawnContainer;
    [SerializeField] private int SpawnPoolCount;
    [SerializeField] private bool AutoExpand;
    

    private void Start()
    {
        pool = new PoolMono<EnemyCharacter>(Prefab,SpawnPoolCount,SpawnContainer,AutoExpand);
        StartCoroutine(SpawnTimer());
    }
    
    public IEnumerator SpawnTimer()
    {
        yield return new WaitForSeconds(5);
        pool.GetFreeElement();
        yield return new WaitForSeconds(5);
        pool.GetFreeElement();
        yield return new WaitForSeconds(5);
        pool.GetFreeElement();
        yield return new WaitForSeconds(5);
        pool.GetFreeElement();
        yield return new WaitForSeconds(5);
        pool.GetFreeElement();

    }
    
}
