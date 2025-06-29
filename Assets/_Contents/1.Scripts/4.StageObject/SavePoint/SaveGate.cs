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

        // パーティクル生成
        // 効果音
    }
}
