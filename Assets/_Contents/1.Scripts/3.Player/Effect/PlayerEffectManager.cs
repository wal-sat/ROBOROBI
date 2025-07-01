using UnityEngine;

public class PlayerEffectManager : MonoBehaviour
{
    [SerializeField] private PlayerScrapManager _playerScrapManager;
    [SerializeField] private PlayerExplosionAnimation _playerExplosionAnimation;

    // ----- Public Methods -----

    public void Death(Vector3 playerDeathPosition, float angleZ)
    {
        _playerScrapManager.DeathExplosion(playerDeathPosition, angleZ);
        _playerExplosionAnimation.DeathExplosion(playerDeathPosition).Forget();
    }
}
