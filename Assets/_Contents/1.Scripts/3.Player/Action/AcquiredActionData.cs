using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/AcquiredActionData", fileName = "AAD_")]
public class AcquiredActionData : ScriptableObject
{
    [SerializeField] private bool _neutral_Grab;
    [SerializeField] private bool _neutral_Accelerate;
    [SerializeField] private bool _neutral_Decelerate;
    [SerializeField] private bool _neutral_Crouch;
    [SerializeField] private bool _neutral_Swap;
    [SerializeField] private bool _neutral_Kick;
    [SerializeField] private bool _neutral_InteractL;
    [SerializeField] private bool _neutral_InteractR;
    [SerializeField] private bool _s_Jump;
    [SerializeField] private bool _s_BigJump;
    [SerializeField] private bool _s_GoDown;
    [SerializeField] private bool _s_DoubleJump;
    [SerializeField] private bool _s_InfiniteJump;

    public Dictionary<ActionKind, bool> AcquiredActionDictionary = new Dictionary<ActionKind, bool>();

    // ----- Life Cycle Methods -----

    private void OnEnable()
    {
        AcquiredActionDictionary.Add(ActionKind.Neutral_Grab, _neutral_Grab);
        AcquiredActionDictionary.Add(ActionKind.Neutral_Accelerate, _neutral_Accelerate);
        AcquiredActionDictionary.Add(ActionKind.Neutral_Decelerate, _neutral_Decelerate);
        AcquiredActionDictionary.Add(ActionKind.Neutral_Crouch, _neutral_Crouch);
        AcquiredActionDictionary.Add(ActionKind.Neutral_Swap, _neutral_Swap);
        AcquiredActionDictionary.Add(ActionKind.Neutral_Kick, _neutral_Kick);
        AcquiredActionDictionary.Add(ActionKind.Neutral_InteractL, _neutral_InteractL);
        AcquiredActionDictionary.Add(ActionKind.Neutral_InteractR, _neutral_InteractR);
        AcquiredActionDictionary.Add(ActionKind.S_Jump, _s_Jump);
        AcquiredActionDictionary.Add(ActionKind.S_BigJump, _s_BigJump);
        AcquiredActionDictionary.Add(ActionKind.S_GoDown, _s_GoDown);
        AcquiredActionDictionary.Add(ActionKind.S_DoubleJump, _s_DoubleJump);
        AcquiredActionDictionary.Add(ActionKind.S_InfiniteJump, _s_InfiniteJump);
    }
}
