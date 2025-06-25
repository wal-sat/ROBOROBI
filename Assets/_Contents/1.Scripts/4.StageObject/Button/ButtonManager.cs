using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    private List<Button> _buttonList = new List<Button>();

    // ----- Public Methods -----

    public void Register(Button button)
    {
        if (button != null && !_buttonList.Contains(button))
        {
            _buttonList.Add(button);
        }
    }

    [Button]
    public void StageObjectInitialize()
    {
        foreach (var button in _buttonList)
        {
            button.ButtonInitialize();
        }
    }
}
