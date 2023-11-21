using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PoolSystems;
using ActionSystems;
public class PointSpawner : MonoBehaviour
{
    protected PoolMono<Item> pool;
    private List<Item> _itemLists= new List<Item>();
    [SerializeField]protected List<Transform> SpawnPositionsList = new List<Transform>();
    [SerializeField] private Item Prefab;
    [SerializeField] private Transform SpawnContainer;
    [SerializeField] private int SpawnPoolCount;
    [SerializeField] private bool AutoExpand;
    [SerializeField] private float SpawnTimeInterval;
    private Item _createdItem;
    private void Start()
    {
        pool = new PoolMono<Item>(Prefab, SpawnPoolCount, SpawnContainer, AutoExpand);
        for (int i = 0; i < SpawnPositionsList.Count; i++)
        {
           _createdItem = pool.GetFreeElement(SpawnContainer);
            _itemLists.Add(_createdItem);
            _createdItem.transform.parent = SpawnPositionsList[i];
            _createdItem.transform.localPosition = new Vector3(0+Random.Range(-0.3f,0.3f),1,0+ Random.Range(-0.3f, 0.3f));
            PLayerStatistics._isItemsActive.Add(true);

        }

    }
    private void Update()
    {
        PLayerStatistics.OnItemDisable(_itemLists,this, "OnCoroutine");
    }
    public void OnCoroutine()
    {
        StartCoroutine(SpawnInterval(SpawnTimeInterval));
    }
    public void OffCoroutine()
    {

        StopCoroutine(SpawnInterval(SpawnTimeInterval));
    }
    public IEnumerator SpawnInterval(float t)
    {
        yield return new WaitForSeconds(t);
        _createdItem = pool.GetFreeElement(SpawnContainer);
        _createdItem.transform.position = SpawnPositionsList[Random.Range(0, SpawnPositionsList.Count)].transform.position;
        _createdItem.transform.localPosition = new Vector3(0 + Random.Range(-0.3f, 0.3f), 1, 0 + Random.Range(-0.3f, 0.3f));
        OffCoroutine();
    }
}
