using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PlayerOverlapRope : MonoBehaviour, IOverlapRope
{
    private List<Rope> _ropeList = new List<Rope>();

    // ----- Public Methods -----

    public void Initialize()
    {
        _ropeList.Clear();
    }

    public void Register(Rope rope)
    {
        if (rope != null && !_ropeList.Contains(rope))
        {
            _ropeList.Add(rope);
        }
    }

    public void Unregister(Rope rope)
    {
        if (rope != null && _ropeList.Contains(rope))
        {
            _ropeList.Remove(rope);
        }
    }

    public bool IsOverlapRope()
    {
        return _ropeList.Count > 0;
    }

    public Transform GetRopeTransform()
    {
        if (_ropeList.Count > 0)
            return _ropeList[0].gameObject.transform;
        else
            return null;
    }
}
