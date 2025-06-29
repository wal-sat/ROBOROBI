using UnityEngine;

public class SavePointManager : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    //[SerializeField] private GearManager _gearManager;

    [HideInInspector] public SavePointBase CurrentSavePoint;

    // ----- Public Methods -----

    public void SetCurrentSavePoint(SavePointBase newSavePoint)
    {
        if (newSavePoint.SavePointIndex >= CurrentSavePoint.SavePointIndex)
        {
            CurrentSavePoint = newSavePoint;
        }

        //_gearManager.OnSave();
    }

    public void TeleportSavePoint(SavePointBase newSavePoint = null)
    {
        if (newSavePoint != null)
        {
            CurrentSavePoint = newSavePoint;
        }

        _player.transform.position = new Vector3(CurrentSavePoint.transform.position.x, CurrentSavePoint.transform.position.y, _player.transform.position.z);
        // Playerのアクションをセットする
    }
}
