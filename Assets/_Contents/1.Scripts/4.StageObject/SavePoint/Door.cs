using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private StageManager _stageManager;
    [SerializeField] private IsTriggerWithPlayer _isTriggerWithPlayer;
    [SerializeField] private ParticleSystem _savePointParticle;

    // ----- Life Cycle Methods -----

    private void Awake()
    {
        _isTriggerWithPlayer.TriggerEnterCallback = TriggerEnter;
    }

    // ----- Private Methods -----

    private void TriggerEnter()
    {
        _stageManager.PlayerEnterDoor();

        _savePointParticle.Play();
        // 効果音
    }
}
