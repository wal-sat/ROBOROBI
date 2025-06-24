using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class GeneratorManager : MonoBehaviour
{
    private List<GeneratorBase> _generatorList = new List<GeneratorBase>();

    // ----- Public Methods -----

    public void Register(GeneratorBase generator)
    {
        _generatorList.Add(generator);
    }

    [Button]
    public void StageObjectInitialize()
    {
        foreach (var generator in _generatorList)
        {
            generator.GeneratorInitialize();
        }
    }
}
