using NaughtyAttributes;
using UnityEngine;

public class SavePointBase : MonoBehaviour
{
    [SerializeField] private bool _isStartGate;

    [HideIf(nameof(_isStartGate))] [SerializeField] protected SavePointManager _savePointManager;
    [HideIf(nameof(_isStartGate))] [SerializeField] protected IsTriggerWithPlayer _isTriggerWithPlayer;
    [HideIf(nameof(_isStartGate))] [SerializeField] protected SaveGateView _saveGateView;
    [HideIf(nameof(_isStartGate))] [SerializeField] protected GameObject _savePointParticle;
    [SerializeField] private SpriteRenderer _sleepCameraArea;

    [SerializeField] public AcquiredActionData AcquiredActionData;
    [SerializeField] public int SavePointIndex;
    [SerializeField] public bool IsFacingRight;

    [HideInInspector] public Vector2 BottomLeftPos;
    [HideInInspector] public Vector2 TopRightPos;

    // ----- Life Cycle Methods -----

    protected virtual void Awake()
    {
        BottomLeftPos = _sleepCameraArea.bounds.min;
        TopRightPos = _sleepCameraArea.bounds.max;
        _sleepCameraArea.sprite = null;
    }
}
