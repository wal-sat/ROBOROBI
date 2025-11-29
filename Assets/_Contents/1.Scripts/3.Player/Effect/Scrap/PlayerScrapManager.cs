using System.Collections.Generic;
using UnityEditor.Localization.Plugins.XLIFF.V20;
using UnityEngine;

public class PlayerScrapManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _scrapPrefabs;

    private const int ScrapPoolCount = 10;
    private const float ScrapOffsetZ = 5f;

    private List<Queue<PlayerScrap>> _scrapPoolList = new List<Queue<PlayerScrap>>();
    private List<Queue<PlayerScrap>> _scrapList = new List<Queue<PlayerScrap>>();

    private int[] _scrapLayerIntArray = new int[3];
    private int _layerIndex;

    // ----- Life Cycle Methods -----

    private void Awake()
    {
        for (int i = 0; i < _scrapLayerIntArray.Length; i++)
        {
            _scrapLayerIntArray[i] = LayerMask.NameToLayer("PlayerScrap_" + i);
        }

        SetUpScrapPool();
    }

    // ----- Public Methods -----

    public void ScrapInitialize()
    {
        for (int i = 0; i < _scrapPrefabs.Length; i++)
        {
            if (_scrapPoolList[i].Count == 0)
            {
                PlayerScrap scrap = _scrapList[i].Dequeue();
                scrap.gameObject.SetActive(false);
                _scrapPoolList[i].Enqueue(scrap);
            }
        }
    }

    public void DeathExplosion(Vector3 playerDeathPosition, float angleZ)
    {
        for (int i = 0; i < _scrapPrefabs.Length; i++)
        {
            PlayerScrap scrap = GetScrapFromPool(i);
            scrap.transform.position = new Vector3(playerDeathPosition.x, playerDeathPosition.y, ScrapOffsetZ + _layerIndex);
            scrap.gameObject.layer = _scrapLayerIntArray[_layerIndex];
            scrap.Explosion(angleZ);

            if (++_layerIndex >= _scrapLayerIntArray.Length)
            {
                _layerIndex = 0;
            }
        }
    }

    public void DestroyAllScraps()
    {
        for (int i = 0; i < _scrapPrefabs.Length; i++)
        {
            int count = _scrapList[i].Count;
            for (int j = 0; j < count; j++)
            {
                PlayerScrap scrap = _scrapList[i].Dequeue();
                scrap.gameObject.SetActive(false);
                _scrapPoolList[i].Enqueue(scrap);
            }
        }
    }

    // ----- Private Methods -----

    private void SetUpScrapPool()
    {
        for (int i = 0; i < _scrapPrefabs.Length; i++)
        {
            _scrapPoolList.Add(new Queue<PlayerScrap>());
            _scrapList.Add(new Queue<PlayerScrap>());
        }

        for (int i = 0; i < _scrapPrefabs.Length; i++)
        {
            for (int j = 0; j < ScrapPoolCount; j++)
            {
                PlayerScrap newScrap = Instantiate(_scrapPrefabs[i], Vector3.zero, Quaternion.identity).GetComponent<PlayerScrap>();
                newScrap.OnDestroyCallBack += ReturnScrapToPool;
                newScrap.ScrapIndex = i;
                newScrap.transform.SetParent(this.transform);
                newScrap.gameObject.SetActive(false);
                _scrapPoolList[i].Enqueue(newScrap);
            }
        }
    }

    private PlayerScrap GetScrapFromPool(int scrapIndex)
    {
        PlayerScrap scrap = _scrapPoolList[scrapIndex].Dequeue();
        scrap.gameObject.SetActive(true);
        _scrapList[scrapIndex].Enqueue(scrap);

        return scrap;
    }

    private void ReturnScrapToPool(int scrapIndex, PlayerScrap scrap)
    {
        Queue<PlayerScrap> tempScrapQueue = new Queue<PlayerScrap>(_scrapList[scrapIndex]);
        _scrapList[scrapIndex].Clear();
        int count = tempScrapQueue.Count;
        for (int i = 0; i < count; i++)
        {
            PlayerScrap playerScrap = tempScrapQueue.Dequeue();
            if (playerScrap == scrap)
            {
                playerScrap.gameObject.SetActive(false);
                playerScrap.transform.position = new Vector3(0f, 0f, playerScrap.transform.position.z);
                _scrapPoolList[scrapIndex].Enqueue(playerScrap);
            }
            else
            {
                _scrapList[scrapIndex].Enqueue(playerScrap);
            }
        }
    }
}
