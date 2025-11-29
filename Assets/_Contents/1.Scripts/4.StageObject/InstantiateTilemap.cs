using System.Collections.Generic;
using UnityEngine;

public class InstantiateTilemap : MonoBehaviour
{
    private void Awake()
    {
        List<Transform> children = new List<Transform>();
        foreach (Transform child in transform)
        {
            children.Add(child);
        }

        foreach (var child in children)
        {
            child.SetParent(this.transform.parent);
        }

        gameObject.SetActive(false);
    }
}
