using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public enum LRObjectState { L, R }

public class LRObjectManager : MonoBehaviour
{
    private const float AnimationDuration = 2.5f;

    private List<LRObject> _lrObjectList = new List<LRObject>();
    private LRObjectState _lrObjectState;

    // ----- Public Methods -----

    public void Register(LRObject lrObject)
    {
        _lrObjectList.Add(lrObject);
    }

    public void LRObjectMove(LRObjectState lrObjectState)
    {
        if (_lrObjectState == lrObjectState) return;
        _lrObjectState = lrObjectState;

        foreach (var lrObject in _lrObjectList)
        {
            lrObject.PlayAnimation(_lrObjectState, AnimationDuration);
        }
    }

    [Button]
    public void Initialize()
    {
        _lrObjectState = LRObjectState.L;

        foreach (var lrObject in _lrObjectList)
        {
            lrObject.Initialize(_lrObjectState);
        }
    }
}
