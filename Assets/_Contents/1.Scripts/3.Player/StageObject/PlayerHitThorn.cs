using UnityEngine;

public class PlayerHitThorn : MonoBehaviour, IHitSpike
{
    [SerializeField] private StageManager _stageManager;

    public void HitSpike(float angleZ)
    {
        _stageManager.PlayerDeath(angleZ).Forget();
    }
}
