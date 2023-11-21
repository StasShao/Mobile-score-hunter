using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PoolSystems;
using ActionSystems;
public class PlayerSpawner : MonoBehaviour
{
    protected PoolMono<Player> pool;
    [SerializeField] private Player Prefab;
    [SerializeField] private Transform SpawnContainer;
    [SerializeField] private int SpawnPoolCount;
    [SerializeField] private bool AutoExpand;
    [SerializeField] private float SpawnTimeInterval;
    private Player _createdPlayer;

    private void Start()
    {
        pool = new PoolMono<Player>(Prefab, SpawnPoolCount, SpawnContainer, AutoExpand);
        _createdPlayer = pool.GetFreeElement(SpawnContainer);
          }
    private void Update()
    {
        OnPlayerStats();
    }
    public virtual void OnPlayerStats()
    {
        PLayerStatistics.OnPLayerDisable(_createdPlayer, this, "OnCoroutine");
        PLayerStatistics.PlayerHit(_createdPlayer, _createdPlayer, "TakeDamage", _createdPlayer);
    }
    public void OnCoroutine()
    {
        StartCoroutine(PlayerSpawnInterval(SpawnTimeInterval));
    }
    public void OffCoroutine()
    {
        
        StopCoroutine(PlayerSpawnInterval(SpawnTimeInterval));
    }
    public IEnumerator PlayerSpawnInterval(float t)
    {
        yield return new WaitForSeconds(t);
        pool.GetFreeElement(SpawnContainer);
        _createdPlayer.SetDirectionPoint(SpawnContainer.position);
        OffCoroutine();
    }
}
