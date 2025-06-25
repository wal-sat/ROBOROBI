using System.Collections.Generic;
using UnityEngine;

public enum MovableObjectDirection { Up, Down, Left, Right, UpLeft, UpRight, DownLeft, DownRight }

public class MovableObjectManager : MonoBehaviour
{
    private List<MovableObject> _movableObjectList = new List<MovableObject>();

    // ----- Public Methods -----

    public void Register(MovableObject movableObject)
    {
        if (movableObject != null && !_movableObjectList.Contains(movableObject))
        {
            _movableObjectList.Add(movableObject);
        }
    }

    public void StageObjectInitialize()
    {
        foreach (var movableObject in _movableObjectList)
        {
            movableObject.MovableObjectInitialize();
        }
    }
}
