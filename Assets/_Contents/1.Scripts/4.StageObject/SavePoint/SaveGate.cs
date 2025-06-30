using UnityEngine;

public class SaveGate : SavePointBase
{
    // ----- Life Cycle Methods -----

    protected override void Awake()
    {
        base.Awake();

        _isTriggerWithPlayer.TriggerEnterCallback = TriggerEnter;
    }

    // ----- Private Methods -----

    private void TriggerEnter()
    {
        _savePointManager.SetCurrentSavePoint(this);
        _saveGateView.GlossSprite().Forget();

        _savePointParticle.Play();
        // 効果音
    }
}
