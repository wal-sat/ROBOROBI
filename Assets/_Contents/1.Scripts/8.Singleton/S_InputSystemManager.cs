using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IInputLockable
{
    // SetInputLock() を呼び出すためのインターフェース
}

public class S_InputSystemManager : Singleton<S_InputSystemManager>
{
    [HideInInspector] public Vector2 LeftDirection { get; private set; }
    [HideInInspector] public Vector2 NormalizedLeftDirection { get; private set; }
    [HideInInspector] public bool IsPushingS { get; private set; }
    [HideInInspector] public bool IsPushingE { get; private set; }
    [HideInInspector] public bool IsPushingW { get; private set; }
    [HideInInspector] public bool IsPushingN { get; private set; }
    [HideInInspector] public bool IsPushingL1 { get; private set; }
    [HideInInspector] public bool IsPushingL2 { get; private set; }
    [HideInInspector] public bool IsPushingR1 { get; private set; }
    [HideInInspector] public bool IsPushingR2 { get; private set; }
    [HideInInspector] public bool IsPushingOption { get; private set; }

    private Dictionary<IInputLockable, bool> _inputLock = new Dictionary<IInputLockable, bool>();

    // ----- Public Methods -----

    /// <summary>
    /// 入力受付の制限の状態を変更する
    /// </summary>
    public void SetInputLock(IInputLockable gameObject, bool isLock)
    {
        if (_inputLock.ContainsKey(gameObject))
        {
            _inputLock[gameObject] = isLock;
        }
        else
        {
            _inputLock.Add(gameObject, isLock);
        }
    }

    public void SetLeftDirection(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            LeftDirection = context.ReadValue<Vector2>();
            NormalizedLeftDirection = NormalizeVector(LeftDirection);
        }
        else if (context.canceled || _inputLock.Values.Any(x => x))
        {
            LeftDirection = Vector2.zero;
            NormalizedLeftDirection = Vector2.zero;
        }
    }
    public void SetIsPushingS(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            IsPushingS = true;
        }
        else if (context.canceled || _inputLock.Values.Any(x => x))
        {
            IsPushingS = false;
        }
    }
    public void SetIsPushingE(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            IsPushingE = true;
        }
        else if (context.canceled || _inputLock.Values.Any(x => x))
        {
            IsPushingE = false;
        }
    }
    public void SetIsPushingW(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            IsPushingW = true;
        }
        else if (context.canceled || _inputLock.Values.Any(x => x))
        {
            IsPushingW = false;
        }
    }
    public void SetIsPushingN(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            IsPushingN = true;
        }
        else if (context.canceled || _inputLock.Values.Any(x => x))
        {
            IsPushingN = false;
        }
    }
    public void SetIsPushingL1(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            IsPushingL1 = true;
        }
        else if (context.canceled || _inputLock.Values.Any(x => x))
        {
            IsPushingL1 = false;
        }
    }
    public void SetIsPushingL2(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            IsPushingL2 = true;
        }
        else if (context.canceled || _inputLock.Values.Any(x => x))
        {
            IsPushingL2 = false;
        }
    }
    public void SetIsPushingR1(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            IsPushingR1 = true;
        }
        else if (context.canceled || _inputLock.Values.Any(x => x))
        {
            IsPushingR1 = false;
        }
    }
    public void SetIsPushingR2(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            IsPushingR2 = true;
        }
        else if (context.canceled || _inputLock.Values.Any(x => x))
        {
            IsPushingR2 = false;
        }
    }
    public void SetIsPushingOption(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            IsPushingOption = true;
        }
        else if (context.canceled || _inputLock.Values.Any(x => x))
        {
            IsPushingOption = false;
        }
    }

    // ----- Private Methods -----

    private Vector2 NormalizeVector(Vector2 vector)
    {
        if (vector.x > 0.5f) return Vector2.right;
        if (vector.x < -0.5f) return Vector2.left;
        if (vector.y > 0.5f) return Vector2.up;
        if (vector.y < -0.5f) return Vector2.down;

        return Vector2.zero;
    }
}