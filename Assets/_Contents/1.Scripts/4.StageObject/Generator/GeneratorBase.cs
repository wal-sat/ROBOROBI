using UnityEngine;
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;


public class GeneratorBase : MonoBehaviour
{
    [SerializeField] private GameObject[] _generateObjects;
    [SerializeField] private float _generateInterval;
    [SerializeField] private bool _onAwake;

    private List<GeneratedObject> _generatedObjectList = new List<GeneratedObject>();
    private CancellationTokenSource _cancellationTokenSource;

    // ----- Life Cycle Methods -----

    private void Awake()
    {
        foreach (var item in _generateObjects)
        {
            if (item.activeSelf)
            {
                item.SetActive(false);
            }
        }   
    }

    private void Start()
    {
        if (_onAwake)
        {
            InitGenerator();
        }
    }

    // ----- Public Methods -----

    public virtual void InitGenerator()
    {
        if (_cancellationTokenSource != null)
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = null;
        }
        _cancellationTokenSource = new CancellationTokenSource();

        ActionLoop(() => GenerateObject(), _cancellationTokenSource.Token).Forget();
    }

    public virtual void EndGenerator()
    {
        if (_cancellationTokenSource != null)
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = null;
        }
    }

    public virtual void GeneratorInitialize()
    {
        if (_cancellationTokenSource != null)
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = null;
        }

        foreach (var generatedObject in _generatedObjectList)
        {
            Destroy(generatedObject.gameObject);
        }
        _generatedObjectList.Clear();
    }

    // ----- Private Methods -----

    protected virtual GameObject InstantiateObject(GameObject randomObject)
    {
        return null;
    }

    protected void GenerateObject()
    {
        GameObject randomObject = GetRandomGeneratedObject();
        if (randomObject == null) return;

        GameObject gameObject = InstantiateObject(randomObject);

        gameObject.SetActive(true);
        GeneratedObject generatedObject = gameObject.GetComponent<GeneratedObject>();
        generatedObject.DestroyCallback = () =>
        {
            _generatedObjectList.Remove(generatedObject);
            Destroy(gameObject);
        };
        _generatedObjectList.Add(generatedObject);
    }

    protected GameObject GetRandomGeneratedObject()
    {
        if (_generateObjects.Length == 0) return null;

        int randomIndex = UnityEngine.Random.Range(0, _generateObjects.Length);
        return _generateObjects[randomIndex];
    }

    protected async UniTaskVoid ActionLoop(Action action, CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            await UniTask.WaitForSeconds(_generateInterval, cancellationToken: token);

            action();
        }
    }
}
