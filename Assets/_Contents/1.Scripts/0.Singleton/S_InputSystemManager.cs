using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class S_InputSystemManager : Singleton<S_InputSystemManager>
{
    [HideInInspector] public Vector2 LeftDirection { get; private set; }
    [HideInInspector] public bool isPushingJump;
    [HideInInspector] public bool isPushingAttack;
    [HideInInspector] public bool isPushingPause;

    private Dictionary<GameObject, bool> _lockInputDictionary = new Dictionary<GameObject, bool>();

    /// <summary>
    /// 入力受付の制限の状態を変更する
    /// </summary>
    public void SetLockInputDictionary(GameObject gameObject, bool canInput)
    {
        if (_lockInputDictionary.ContainsKey(gameObject))
        {
            _lockInputDictionary[gameObject] = canInput;
        }
        else
        {
            _lockInputDictionary.Add(gameObject, canInput);
        }
    }

    // ---------------- Player Map ----------------

    public void SetLeftDirection(InputAction.CallbackContext context)
    {
        if (context.performed) LeftDirection = context.ReadValue<Vector2>();
        else if (context.canceled) LeftDirection = Vector2.zero;

        if (_lockInputDictionary.Values.Any(x => x)) LeftDirection = Vector2.zero;
    }
    public void Pause(InputAction.CallbackContext context)
    {
        if (context.performed) isPushingPause = true;
        else if (context.canceled) isPushingPause = false;

        if (_lockInputDictionary.Values.Any(x => x)) isPushingPause = false;
    }
}