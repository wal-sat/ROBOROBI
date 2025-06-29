using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private StageManager _stageManager;
    [SerializeField] private IsTriggerWithPlayer _isTriggerWithPlayer;

    // ----- Life Cycle Methods -----

    private void Awake()
    {
        _isTriggerWithPlayer.TriggerEnterCallback = TriggerEnter;
    }

    // ----- Private Methods -----

    private void TriggerEnter()
    {
        _stageManager.Door();

        // パーティクル生成
        // 効果音
    }
}
