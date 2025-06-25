using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RopeManager : MonoBehaviour
{
    private List<Rope> _ropeList = new List<Rope>();

    // ----- Public Methods -----

    public void Register(Rope rope)
    {
        if (rope != null && !_ropeList.Contains(rope))
        {
            _ropeList.Add(rope);
        }
    }

    public bool IsOverlapRope()
    {
        return _ropeList.Any(item => item.IsOverlapPlayer);
    }

    public Transform GetRopeTransform()
    {
        return _ropeList.First(item => item.IsOverlapPlayer).gameObject.transform;
    }
}
