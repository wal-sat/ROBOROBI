using UnityEngine;

public class Rope : MonoBehaviour
{
    [SerializeField] private RopeManager _ropeManager;
    [SerializeField] private IsTriggerWithPlayer _isTriggerWithPlayer;
    
    [HideInInspector] public bool IsOverlapPlayer;

    // ----- Life Cycle Methods -----

    private void Awake()
    {
        _ropeManager.Register(this);

        _isTriggerWithPlayer.TriggerEnterCallback = TriggerEnter;
        _isTriggerWithPlayer.TriggerExitCallback = TriggerExit;
    }

    // ----- Private Methods -----

    private void TriggerEnter()
    {
        IsOverlapPlayer = true;
    }

    private void TriggerExit()
    {
        IsOverlapPlayer = false;
    }
}
