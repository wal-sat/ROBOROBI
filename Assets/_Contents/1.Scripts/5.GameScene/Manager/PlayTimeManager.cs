using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;

public class PlayTimeManager : MonoBehaviour
{
    private bool _isMeasuringTime;
    private float _time;

    // ----- Life Cycle Methods -----

    private void Update()
    {
        if (_isMeasuringTime)
        {
            _time += Time.deltaTime;
        }
    }

    // ----- Public Methods -----

    public void StartTimer()
    {
        _isMeasuringTime = true;
    }

    public void StopTimer()
    {
        _isMeasuringTime = false;
    }

    public void ResetTimer()
    {
        _isMeasuringTime = false;
        _time = 0;
    }

    public int GetPlayTimeInt()
    {
        return (int)_time;
    }

    public string GetPlayTimeString()
    {
        int hours = (int)_time / 3600;
        int minutes = (int)(_time % 3600) / 60;
        int seconds = (int)_time % 60;

        return string.Format("{0}:{1:D2}:{2:D2}", hours, minutes, seconds);
    }
}
