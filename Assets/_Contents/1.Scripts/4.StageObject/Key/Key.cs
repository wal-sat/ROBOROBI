using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField] private StageManager _stageManager;
    [SerializeField] private KeyView _keyView;
    [SerializeField] private IsTriggerWithPlayer _isTriggerWithPlayer;
    [SerializeField] private ParticleSystem _keyDefaultParticle;
    [SerializeField] private ParticleSystem _savePointParticle;

    // ----- Life Cycle Methods -----

    private void Awake()
    {
        _isTriggerWithPlayer.TriggerEnterCallback += TriggerEnter;

        _keyView.ChangeSprite(true);
        _keyDefaultParticle.Play();
    }

    // ----- Private Methods -----

    private void TriggerEnter()
    {
        _keyView.ChangeSprite(false);
        _keyDefaultParticle.Stop();
        _savePointParticle.Play();

        _stageManager.StageClear().Forget();
    }
}
