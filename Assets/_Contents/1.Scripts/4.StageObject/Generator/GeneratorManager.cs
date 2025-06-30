using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class GeneratorManager : MonoBehaviour
{
    private List<GeneratorBase> _generatorList = new List<GeneratorBase>();

    // ----- Public Methods -----

    public void Register(GeneratorBase generator)
    {
        if (generator != null && !_generatorList.Contains(generator))
        {
            _generatorList.Add(generator);
        }
    }

    public void StageObjectInitialize()
    {
        foreach (var generator in _generatorList)
        {
            generator.GeneratorInitialize();
        }
    }
}
