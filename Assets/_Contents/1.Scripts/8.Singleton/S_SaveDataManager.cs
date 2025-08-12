using System.Collections.Generic;
using System.IO;
using UnityEngine;
using NaughtyAttributes;

public class S_SaveDataManager : Singleton<S_SaveDataManager>
{
    private const int SaveDataCount = 3;

    private SaveData[] _saveDatas = new SaveData[SaveDataCount];
    private string[] _filePaths = new string[SaveDataCount];
    private int _currentSaveDataIndex;

    // ----- Life Cycle Methods -----

    protected override void Awake()
    {
        base.Awake();
        if (!_isValid) return;

        for (int i = 0; i < SaveDataCount; i++)
        {
            _filePaths[i] = $"{Application.dataPath}/_SaveData/SaveData{i}.json";

            LoadAll();
        }
    }

    // ----- Public Methods -----

    public void SetCurrentSaveDataIndex(int index)
    {
        if (index < 0 || index >= SaveDataCount) return;

        _currentSaveDataIndex = index;
    }

    public void MakeSaveData()
    {
        _saveDatas[_currentSaveDataIndex] = new SaveData();

        Save();
    }

    public void Save()
    {
        if (_saveDatas[_currentSaveDataIndex] != null)
        {
            string json = JsonUtility.ToJson(_saveDatas[_currentSaveDataIndex], true);
            StreamWriter wr = new StreamWriter(_filePaths[_currentSaveDataIndex], false);
            wr.WriteLine(json);
            wr.Close();
        }

    }

    public void Load()
    {
        if (File.Exists(_filePaths[_currentSaveDataIndex]))
        {
            StreamReader rd = new StreamReader(_filePaths[_currentSaveDataIndex]);
            string json = rd.ReadToEnd();
            rd.Close();

            _saveDatas[_currentSaveDataIndex] = JsonUtility.FromJson<SaveData>(json);
        }
        else
        {
            _saveDatas[_currentSaveDataIndex] = null;
        }
    }

    // ----- Private Methods -----

    private void LoadAll()
    {
        for (int i = 0; i < SaveDataCount; i++)
        {
            if (File.Exists(_filePaths[i]))
            {
                StreamReader rd = new StreamReader(_filePaths[i]);
                string json = rd.ReadToEnd();
                rd.Close();

                _saveDatas[i] = JsonUtility.FromJson<SaveData>(json);
            }
            else
            {
                _saveDatas[i] = null;
            }
        }
    }
}
