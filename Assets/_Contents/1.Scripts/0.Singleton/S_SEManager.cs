using System.Collections.Generic;
using UnityEngine;

public class S_SEManager : Singleton<S_SEManager>
{
    [System.Serializable] class SEInfo
    {
        public string name;
        public AudioClip audioClip;
    }

    [SerializeField] float MAX_VOLUME;
    [SerializeField] List<SEInfo> SEList = new List<SEInfo>();

    private AudioSource[] _audioSourceList = new AudioSource[20];
    private Dictionary<string, SEInfo> _soundDictionary = new Dictionary<string, SEInfo>();
    private float _volume;
 
    public override void Awake()
    {
        base.Awake();

        for (var i = 0; i < _audioSourceList.Length; ++i)
        {
            _audioSourceList[i] = gameObject.AddComponent<AudioSource>();
            _audioSourceList[i].priority = 128;
        }
 
        foreach (var SEInfo in SEList)
        {
            _soundDictionary.Add(SEInfo.name, SEInfo);
        }
    }

    /// <summary>
    /// SEを再生する
    /// </summary>
    public void Play(string name)
    {
        if (_soundDictionary.TryGetValue(name, out var SEInfo))
        {
            var audioSource = GetUnusedAudioSource();
            if (audioSource == null) return;
            audioSource.PlayOneShot(SEInfo.audioClip);
        }
    }

    /// <summary>
    /// SEの音量を変更する
    /// </summary>
    public void ChangeVolume(float volume)
    {
        _volume = volume * MAX_VOLUME;
        foreach (var audioSource in _audioSourceList)
        {
            audioSource.volume = _volume;
        }
    }

    /// <summary>
    /// 未使用のAudioSourceを取得する
    /// </summary>
    private AudioSource GetUnusedAudioSource()
    {
        for (var i = 0; i < _audioSourceList.Length; ++i)
        {
            if (_audioSourceList[i].isPlaying == false) return _audioSourceList[i];
        }

        Debug.LogError("未使用のAudioSourceがありません");
        return null;
    }
}