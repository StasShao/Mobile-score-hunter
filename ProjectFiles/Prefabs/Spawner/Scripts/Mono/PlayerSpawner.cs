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
    public Player createdPlayer;
    public ScoreManager scoreManager;
    private Pointer _pointer = new Pointer();

    private void Start()
    {
        pool = new PoolMono<Player>(Prefab, SpawnPoolCount, SpawnContainer, AutoExpand);
        createdPlayer = pool.GetFreeElement(SpawnContainer);
        Debug.Log(scoreManager.savePath);
        Debug.Log(scoreManager.highScore);
    }
    private void Update()
    {
        OnPlayerStats();
        OnProgressDataSave();
    }
    public virtual void OnProgressDataSave()
    {
        _pointer.OnProgressPointsSave(scoreManager,createdPlayer);
        
    }
    public virtual void OnPlayerStats()
    {
        PLayerStatistics.OnPLayerDisable(createdPlayer, this, "OnCoroutine");
        PLayerStatistics.PlayerHit(createdPlayer, createdPlayer, "TakeDamage", createdPlayer);
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
        createdPlayer.SetDirectionPoint(SpawnContainer.position);
        OffCoroutine();
    }
}
