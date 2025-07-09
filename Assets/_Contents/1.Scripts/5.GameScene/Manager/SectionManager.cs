using System;
using UnityEngine;

public class SectionManager : MonoBehaviour
{
    [Serializable] class SectionInfo
    {
        [SerializeField] public GameObject Section;
        [SerializeField] public SavePointBase SavePoint;
    }

    [SerializeField] private SavePointManager _savePointManager;
    [SerializeField] private SectionInfo[] _sections;

    private int _currentSectionIndex;

    // ----- Public Methods -----

    public SavePointBase NextSection()
    {
        return ChangeSection(++_currentSectionIndex);
    }

    public SavePointBase ChangeSection(int sectionIndex)
    {
        _currentSectionIndex = sectionIndex;

        for (int i = 0; i < _sections.Length; i++)
        {
            _sections[i].Section.SetActive(false);
        }

        _sections[_currentSectionIndex].Section.SetActive(true);

        return _sections[_currentSectionIndex].SavePoint;
    }
}
