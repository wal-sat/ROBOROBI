using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class PlayerScrapManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _scraps;

    private const float ScrapOffsetZ = 5f;

    private List<GameObject> _scrapList = new List<GameObject>();
    private int[] _scrapLayerIntArray = new int[3];
    private int _layerIndex;

    // ----- Life Cycle Methods -----

    private void Awake()
    {
        for (int i = 0; i < _scrapLayerIntArray.Length; i++)
        {
            _scrapLayerIntArray[i] = LayerMask.NameToLayer("PlayerScrap_" + i);
        }
    }

    // ----- Public Methods -----

    public void DeathExplosion(Vector3 playerDeathPosition, float angleZ)
    {
        foreach (GameObject scrap in _scraps)
        {
            GameObject newScrap = Instantiate(scrap, new Vector3(playerDeathPosition.x, playerDeathPosition.y, ScrapOffsetZ + _layerIndex), Quaternion.identity);
            newScrap.layer = _scrapLayerIntArray[_layerIndex];
            newScrap.SetActive(true);
            newScrap.GetComponent<PlayerScrap>().Explosion(angleZ, RemoveScrapFromList);

            AddScrapToList(newScrap);

            if (++_layerIndex >= _scrapLayerIntArray.Length)
            {
                _layerIndex = 0;
            }
        }
    }

    public void DestroyAllScraps()
    {
        foreach (GameObject scrap in _scrapList)
        {
            Destroy(scrap);
        }
        _scrapList.Clear();
    }

    // ----- Private Methods -----

    private void AddScrapToList(GameObject scrap)
    {
        _scrapList.Add(scrap);
    }

    private void RemoveScrapFromList(GameObject scrap)
    {
        _scrapList.Remove(scrap);
    }
}
