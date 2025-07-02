using NaughtyAttributes;
using UnityEngine;

public class SavePointBase : MonoBehaviour
{
    [SerializeField] private bool _isStartGate;

    [HideIf(nameof(_isStartGate))] [SerializeField] protected SavePointManager _savePointManager;
    [HideIf(nameof(_isStartGate))] [SerializeField] protected IsTriggerWithPlayer _isTriggerWithPlayer;
    [HideIf(nameof(_isStartGate))] [SerializeField] protected SaveGateView _saveGateView;
    [HideIf(nameof(_isStartGate))] [SerializeField] protected ParticleSystem _savePointParticle;

    [SerializeField] public AcquiredActionData AcquiredActionData;
    [SerializeField] public StageAreaBase SleepCameraArea;
    [SerializeField] public int SavePointIndex;
    [SerializeField] public bool IsFacingRight;
}
