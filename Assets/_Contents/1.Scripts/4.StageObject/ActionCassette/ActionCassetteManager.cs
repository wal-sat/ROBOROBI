using UnityEngine;
using System.Collections.Generic;
using System;

public class ActionCassetteManager : MonoBehaviour
{
    [Serializable]
    private class ActionInfo
    {
        public ActionKind ActionKind;
        public string ActionName;
        public Sprite ActionIcon;
    }

    [SerializeField] private PlayerActionManager _playerActionManager;
    [SerializeField] private ActionInfo[] _actionInfos;

    private List<ActionCassette> _actionCassetteList = new List<ActionCassette>();
    private Dictionary<ActionKind, ActionInfo> _actionInfoDictionary = new Dictionary<ActionKind, ActionInfo>();

    // ----- Life Cycle Methods -----

    private void Awake()
    {
        for (int i = 0; i < _actionInfos.Length; i++)
        {
            _actionInfoDictionary.Add(_actionInfos[i].ActionKind, _actionInfos[i]);
        }
    }

    // ----- Public Methods -----

    public void Register(ActionCassette actionCassette)
    {
        if (actionCassette != null && !_actionCassetteList.Contains(actionCassette))
        {
            _actionCassetteList.Add(actionCassette);
        }
    }

    public void StageObjectInitialize()
    {
        foreach (var actionCassette in _actionCassetteList)
        {
            actionCassette.ActionCassetteInitialize();
        }
    }

    public void AcquireAction(ActionKind actionKind)
    {
        _playerActionManager.AcquireAction(actionKind);
        // UI表示の処理
    }

    public void ForgetAction(ActionKind actionKind)
    {
        _playerActionManager.ForgetAction(actionKind);
        // UI表示の処理
    }
}
