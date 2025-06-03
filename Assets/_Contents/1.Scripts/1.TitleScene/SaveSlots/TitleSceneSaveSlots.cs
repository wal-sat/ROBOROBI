using System;
using UnityEngine;

public class TitleSceneSaveSlots : MonoBehaviour
{
    [SerializeField] private TitleSceneUIToolkit _titleSceneUIToolkit;

    public Action<TitleSceneState> OnChangeTitleSceneState;

    private bool _isSelecttingBackOption;

    private int _saveSlotsIndex;
    int SaveSlotsIndex
    {
        get => _saveSlotsIndex;
        set
        {
            _saveSlotsIndex = Mathf.Clamp(value, 0, 2);

            _titleSceneUIToolkit.SelectSaveSlotOption(_saveSlotsIndex, _isSelecttingBackOption);
        }
    }

    // ----- Public Methods -----

    public void Up()
    {
        _isSelecttingBackOption = false;
        SaveSlotsIndex = SaveSlotsIndex;
    }
    public void Down()
    {
        _isSelecttingBackOption = true;
        SaveSlotsIndex = SaveSlotsIndex;
    }
    public void Left()
    {
        SaveSlotsIndex--;
    }
    public void Right()
    {
        SaveSlotsIndex++;
    }
    public void Submit()
    {
        SaveSlotsSubmit(SaveSlotsIndex);
    }
    public void Cancel()
    {
        BackMenu();
    }

    public void Initialize()
    {
        _isSelecttingBackOption = false;
        SaveSlotsIndex = 0;
    }

    // ----- Private Methods -----

    private void SaveSlotsSubmit(int index)
    {
        if (_isSelecttingBackOption)
        {
            BackMenu();
            return;
        }

        switch (index)
        {
            case 0:
                
                break;
            case 1:
                
                break;
            case 2:
                
                break;
        }
    }

    private void BackMenu()
    {
        _titleSceneUIToolkit.SelectMenuOption(-1);
        OnChangeTitleSceneState?.Invoke(TitleSceneState.Menu);
    }
}
