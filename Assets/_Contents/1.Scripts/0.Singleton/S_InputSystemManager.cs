using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public enum ActionMapKind { Player, UI }

public class S_InputSystemManager : Singleton<S_InputSystemManager>
{
    [HideInInspector] public Vector2 playerMove;
    [HideInInspector] public float dashDirection;
    [HideInInspector] public bool isPushingJump;
    [HideInInspector] public bool isPushingAttack;
    [HideInInspector] public bool isPushingPause;

    [HideInInspector] public Vector2 UIMove;
    [HideInInspector] public bool isPushingSubmit;
    [HideInInspector] public bool isPushingCancel;

    private PlayerInput playerInput;
    private Dictionary<GameObject, bool> _lockInputDictionary = new Dictionary<GameObject, bool>();

    public override void Awake()
    {
        base.Awake();

        playerInput = GetComponent<PlayerInput>();
    }
    
    /// <summary>
    /// アクションマップを切り替える
    /// </summary>
    public void SwitchActionMap(ActionMapKind actionMap)
    {
        if (playerInput.currentActionMap.name == actionMap.ToString()) return;

        playerInput.SwitchCurrentActionMap(actionMap.ToString());
    }

    /// <summary>
    /// 入力受付の制限の状態を変更する
    /// </summary>
    public void SetLockInputDictionary(GameObject gameObject, bool canInput)
    {
        if (_lockInputDictionary.ContainsKey(gameObject)) _lockInputDictionary[gameObject] = canInput;
        else _lockInputDictionary.Add(gameObject, canInput);

        if (_lockInputDictionary.Values.Any(x => x)) AllInputReset();
    }

    // ---------------- Player Map ----------------

    public void Move(InputAction.CallbackContext context)
    {
        if (context.performed) playerMove = context.ReadValue<Vector2>();
        else if (context.canceled) playerMove = Vector2.zero;

        if (_lockInputDictionary.Values.Any(x => x)) playerMove = Vector2.zero;
    }
    public void Dash(InputAction.CallbackContext context)
    {
        if (context.performed) dashDirection = context.ReadValue<float>();
        else if (context.canceled) dashDirection = 0;

        if (_lockInputDictionary.Values.Any(x => x)) dashDirection = 0;
    }
    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed) isPushingJump = true;
        else if (context.canceled) isPushingJump = false;

        if (_lockInputDictionary.Values.Any(x => x)) isPushingJump = false;
    }
    public void Attack(InputAction.CallbackContext context)
    {
        if (context.performed) isPushingAttack = true;
        else if (context.canceled) isPushingAttack = false;

        if (_lockInputDictionary.Values.Any(x => x)) isPushingAttack = false;
    }
    public void Pause(InputAction.CallbackContext context)
    {
        if (context.performed) isPushingPause = true;
        else if (context.canceled) isPushingPause = false;

        if (_lockInputDictionary.Values.Any(x => x)) isPushingPause = false;
    }

    // ---------------- UI Map ----------------

    public void Navigate(InputAction.CallbackContext context)
    {
        if (context.performed) UIMove = VectorNormalization(context.ReadValue<Vector2>());
        else if (context.canceled) UIMove = Vector2.zero;

        if (_lockInputDictionary.Values.Any(x => x)) UIMove = Vector2.zero;
    }
    public void Submit(InputAction.CallbackContext context)
    {
        if (context.performed) isPushingSubmit = true;
        else if (context.canceled) isPushingSubmit = false;

        if (_lockInputDictionary.Values.Any(x => x)) isPushingSubmit = false;
    }
    public void Cancel(InputAction.CallbackContext context)
    {
        if (context.performed) isPushingCancel = true;
        else if (context.canceled) isPushingCancel = false;

        if (_lockInputDictionary.Values.Any(x => x)) isPushingCancel = false;
    }

    // ----------------------------------------
    
    private void AllInputReset()
    {
        playerMove = Vector2.zero;
        dashDirection = 0;
        isPushingJump = false;
        isPushingAttack = false;
        isPushingPause = false;

        UIMove = Vector2.zero;
        isPushingSubmit = false;
        isPushingCancel = false;
    }
    private Vector2 VectorNormalization(Vector2 vector2)
    {
        if (vector2.x > 0.5f) return Vector2.right;
        if (vector2.x < -0.5f) return Vector2.left;
        if (vector2.y > 0.5f) return Vector2.up;
        if (vector2.y < -0.5f) return Vector2.down;
        return Vector2.zero;
    }
}