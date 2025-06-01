using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    private List<Background> _backgrounds = new List<Background>();

    // ----- Public Methods -----

    public void Register(Background background)
    {
        _backgrounds.Add(background);
    }

    public void Initialize()
    {
        foreach (var background in _backgrounds)
        {
            if (background.gameObject.activeSelf) background.Initialize();
        }
    }
}
