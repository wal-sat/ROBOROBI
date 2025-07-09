using UnityEngine;

public class DeathCountManager : MonoBehaviour
{
    public int DeathCount { get; set; }

    // ----- Public Methods -----

    public void IncrementDeathCount()
    {
        DeathCount++;
    }

    public void ResetDeathCount()
    {
        DeathCount = 0;
    }
}
