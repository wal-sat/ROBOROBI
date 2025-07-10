using UnityEngine;

public enum ClearKind { Key, GoldGear }

[CreateAssetMenu(menuName = "ScriptableObject/StageData", fileName = "SD_")]
public class StageData : ScriptableObject
{
    [SerializeField] public SceneKind SceneKind;
    [SerializeField] public int WorldIndex;
    [SerializeField] public int StageIndex;
    [SerializeField] public string WorldName;
    [SerializeField] public string StageName;
    [SerializeField] public ClearKind ClearKind;

    [HideInInspector] public bool IsClear { get; set; }
    [HideInInspector] public bool[] IsAcquiredGears = new bool[5];

    private int _totalDeathCount;
    private int _minimumDeathCount;
    private int _totalPlayTime;
    private int _fastestClearTime;

    // ----- Life Cycle Methods -----

    private void OnEnable()
    {
        Initialize();
    }

    // ----- Public Methods -----

    public void Initialize()
    {
        IsClear = false;
        for (int i = 0; i < IsAcquiredGears.Length; i++)
        {
            IsAcquiredGears[i] = false;
        }
        _totalDeathCount = 0;
        _minimumDeathCount = -1;
        _totalPlayTime = 0;
        _fastestClearTime = -1;
    }

    public void SetDeathCount(int count, bool isCheckMinimum)
    {
        _totalDeathCount += count;

        if (isCheckMinimum)
        {
            if (count < _minimumDeathCount || _minimumDeathCount == -1)
            {
                _minimumDeathCount = count;
            }
        }
    }

    public void SetPlayTime(int time, bool isCheckFastest)
    {
        _totalPlayTime = time;

        if (isCheckFastest)
        {
            if (time < _fastestClearTime || _fastestClearTime == -1)
            {
                _fastestClearTime = time;
            }
        }
    }

    public string GetTotalDeathCountString()
    {
        return _totalDeathCount.ToString();
    }

    public string GetMinimumDeathCountString()
    {
        return _minimumDeathCount.ToString();
    }

    public string GetTotalPlayTimeString()
    {
        int hours = _totalPlayTime / 3600;
        int minutes = (_totalPlayTime % 3600) / 60;
        int seconds = _totalPlayTime % 60;

        return string.Format("{0}:{1:D2}:{2:D2}", hours, minutes, seconds);
    }

    public string GetFastestClearTimeString()
    {
        int hours = _fastestClearTime / 3600;
        int minutes = (_fastestClearTime % 3600) / 60;
        int seconds = _fastestClearTime % 60;

        return string.Format("{0}:{1:D2}:{2:D2}", hours, minutes, seconds);
    }
}
