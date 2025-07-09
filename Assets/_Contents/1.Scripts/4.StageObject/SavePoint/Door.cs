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
        _stageManager.PlayerEnterDoor().Forget();

        _savePointParticle.Play();
        S_SEManager.Instance.Play("s_saveGate");
    }
}
