using UnityEngine;

public class SaveGate : SavePointBase
{
    // ----- Life Cycle Methods -----

    private void Awake()
    {
        _isTriggerWithPlayer.TriggerEnterCallback = TriggerEnter;
    }

    // ----- Private Methods -----

    private void TriggerEnter()
    {
        _savePointManager.SetCurrentSavePoint(this);
        _saveGateView.GlossSprite().Forget();

        _savePointParticle.Play();
        S_SEManager.Instance.Play("s_saveGate");
    }
}
