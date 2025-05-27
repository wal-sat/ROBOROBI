using UnityEngine;
using System;
using System.Linq;
using System.Threading;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public class S_BGMManager : Singleton<S_BGMManager>
{
    enum BGMStatus {  None, FadeIn, Play, FadeOut, Pause, Stop }

    [Serializable] class BGMInfo
    {
        [SerializeField] public string Name;
        [SerializeField] public AudioClip[] BGMClip;

        [HideInInspector] public AudioSource audioSource;
        [HideInInspector] public CancellationTokenSource cancellationTokenSource;
        [HideInInspector] public BGMStatus bgmStatus = BGMStatus.None;
        private int _clipIndex = 0;
        public int clipIndex
        {
            get => _clipIndex;
            set
            {
                _clipIndex = value;
                if (_clipIndex >= BGMClip.Length) _clipIndex = 0;

                if (audioSource != null) audioSource.clip = BGMClip[_clipIndex];
            }
        }

        public bool IsFinished()
        {
            return audioSource.time == 0.0f && !audioSource.isPlaying;
        }
    }

    [SerializeField] private float _maxBGMVolume;
    [SerializeField] private List<BGMInfo> _bgmList = new List<BGMInfo>();

    private AudioSource[] _audioSourceList = new AudioSource[5];
    private Dictionary<string, BGMInfo> _bgmDictionary = new Dictionary<string, BGMInfo>();
    private float _volume;
 
    public override void Awake()
    {
        base.Awake();

        for (int i = 0; i < _bgmList.Count; ++i)
        {
            _bgmDictionary.Add(_bgmList[i].Name, _bgmList[i]);
        }
    
        for (int i = 0; i < _audioSourceList.Length; ++i)
        {
            _audioSourceList[i] = gameObject.AddComponent<AudioSource>();
            _audioSourceList[i].loop = false;
            _audioSourceList[i].priority = 1;
            _audioSourceList[i].volume = 0;
        }
    }
    public void Update()
    {
        foreach (var item in _bgmDictionary.Values)
        {
            if (item.audioSource == null) { continue; }
            
            if ((item.bgmStatus == BGMStatus.FadeIn || item.bgmStatus == BGMStatus.Play || item.bgmStatus == BGMStatus.FadeOut) && item.IsFinished())
            {
                item.clipIndex++;
                item.audioSource.Play();
            }
        }   
    }

    /// <summary>
    /// BGMを再生する
    /// </summary>
    /// <param name="name">BGMの名前</param>
    /// <param name="fadeTime">フェードインの時間</param>
    public void Play(string name, float fadeTime)
    {
        List<string> playingBGM = GetPlayingBGM();
        foreach (var item in playingBGM) 
        {
            if (item != name) Stop(item, fadeTime);
        }
        if (playingBGM.Any(x => x == name)) { return; }
        
        if (_bgmDictionary.TryGetValue(name, out var BGMInfo))
        {
            if (BGMInfo.cancellationTokenSource != null)
            {
                BGMInfo.cancellationTokenSource.Cancel();
            }
            BGMInfo.cancellationTokenSource = new CancellationTokenSource();

            if (BGMInfo.bgmStatus == BGMStatus.FadeOut || BGMInfo.bgmStatus == BGMStatus.Pause)
            {
                ResumeAsync(BGMInfo, fadeTime, BGMInfo.cancellationTokenSource.Token);
            }
            else if (BGMInfo.bgmStatus == BGMStatus.Stop || BGMInfo.bgmStatus == BGMStatus.None)
            {
                BGMInfo.audioSource = GetUnusedAudioSource();
                if (BGMInfo.audioSource == null) { return; }

                BGMInfo.clipIndex = BGMInfo.clipIndex;
                PlayAsync(BGMInfo, fadeTime, BGMInfo.cancellationTokenSource.Token);
            }
        }
    }
    private async void PlayAsync(BGMInfo BGMInfo, float fadeTime, CancellationToken token)
    {
        BGMInfo.bgmStatus = BGMStatus.FadeIn;
        BGMInfo.audioSource.Play();
        float currentVolume = BGMInfo.audioSource.volume;

        for (int i = 0; i < 100; i++)
        {
            BGMInfo.audioSource.volume += (_volume - currentVolume) / 100;
            if (BGMInfo.audioSource.volume > _volume)
            {
                BGMInfo.audioSource.volume = _volume;
            }

            try { await UniTask.Delay(TimeSpan.FromSeconds(fadeTime / 100), cancellationToken: token); }
            catch { return; }
        }
        BGMInfo.audioSource.volume = _volume;
        BGMInfo.cancellationTokenSource = null;
        BGMInfo.bgmStatus = BGMStatus.Play;
    }
    private async void ResumeAsync(BGMInfo BGMInfo, float fadeTime, CancellationToken token)
    {
        BGMInfo.bgmStatus = BGMStatus.FadeIn;
        BGMInfo.audioSource.pitch = 1;
        float currentVolume = BGMInfo.audioSource.volume;

        for (int i = 0; i < 100; i++)
        {
            BGMInfo.audioSource.volume += ( _volume - currentVolume ) / 100;

            try { await UniTask.Delay(TimeSpan.FromSeconds(fadeTime / 100), cancellationToken: token); }
            catch { return; }
        }
        BGMInfo.audioSource.volume = _volume;
        BGMInfo.cancellationTokenSource = null;
        BGMInfo.bgmStatus = BGMStatus.Play;
    }

    /// <summary>
    /// BGMを停止する
    /// </summary>
    /// <param name="name">BGMの名前</param>
    /// <param name="fadeTime">フェードアウトの時間</param>
    public void Stop(string name, float fadeTime)
    {
        if (_bgmDictionary.TryGetValue(name, out var BGMInfo))
        {
            if (BGMInfo.cancellationTokenSource != null)
            {
                BGMInfo.cancellationTokenSource.Cancel();
            }    
            BGMInfo.cancellationTokenSource = new CancellationTokenSource();

            StopAsync(BGMInfo, fadeTime, BGMInfo.cancellationTokenSource.Token);
        }
    }
    private async void StopAsync(BGMInfo BGMInfo, float fadeTime, CancellationToken token)
    {
        BGMInfo.bgmStatus = BGMStatus.FadeOut;
        float currentVolume = BGMInfo.audioSource.volume;

        for (int i = 0; i < 100; i++)
        {
            BGMInfo.audioSource.volume -= currentVolume / 100;

            try { await UniTask.Delay(TimeSpan.FromSeconds(fadeTime / 100), cancellationToken: token); }
            catch { return; }
        }
        BGMInfo.audioSource.volume = 0;
        BGMInfo.audioSource.Stop();
        BGMInfo.audioSource = null;
        BGMInfo.cancellationTokenSource = null;
        BGMInfo.bgmStatus = BGMStatus.Stop;
    }

    /// <summary>
    /// BGMを一時停止する
    /// </summary>
    /// <param name="name">BGMの名前</param>
    /// <param name="fadeTime">フェードアウトの時間</param>
    public void Pause(string name, float fadeTime)
    {
        if (_bgmDictionary.TryGetValue(name, out var BGMInfo))
        {
            if (BGMInfo.cancellationTokenSource != null)
            {
                BGMInfo.cancellationTokenSource.Cancel();
            }
            BGMInfo.cancellationTokenSource = new CancellationTokenSource();

            PauseAsync(BGMInfo, fadeTime, BGMInfo.cancellationTokenSource.Token);
        }
    }
    private async void PauseAsync(BGMInfo BGMInfo, float fadeTime, CancellationToken token)
    {
        BGMInfo.bgmStatus = BGMStatus.FadeOut;
        float currentVolume = BGMInfo.audioSource.volume;

        for (int i = 0; i < 100; i++)
        {
            BGMInfo.audioSource.volume -= currentVolume / 100;
            
            try { await UniTask.Delay(TimeSpan.FromSeconds(fadeTime / 100), cancellationToken: token); }
            catch { return; }
        }
        BGMInfo.audioSource.volume = 0;
        BGMInfo.audioSource.pitch = 0;
        BGMInfo.cancellationTokenSource = null;
        BGMInfo.bgmStatus = BGMStatus.Pause;
    }

    /// <summary>
    /// BGMをミュートする
    /// </summary>
    public void Mute()
    {
        foreach (var item in _bgmDictionary.Values)
        {
            item.audioSource.pitch = 0;
        }
    }
    /// <summary>
    /// BGMのミュートを解除する
    /// </summary>
    public void UnMute()
    {
        foreach (var item in _bgmDictionary.Values)
        {
            item.audioSource.pitch = 1;
        }
    }

    /// <summary>
    /// BGMの音量を変更する
    /// </summary>
    public void ChangeVolume(float volume)
    {
        _volume = volume * _maxBGMVolume;
        foreach (var item in _bgmDictionary.Values)
        {
            if (item.bgmStatus == BGMStatus.FadeIn || item.bgmStatus == BGMStatus.Play)
            {
                item.audioSource.volume = _volume;
            }
        }
    }

    /// <summary>
    /// 再生中のBGMを取得する
    /// </summary>
    private List<string> GetPlayingBGM()
    {
        List<string> playingBGM = new List<string>();
        foreach (var item in _bgmDictionary.Values)
        {
            if (item.bgmStatus == BGMStatus.FadeIn || item.bgmStatus == BGMStatus.Play)
            {
                playingBGM.Add(item.Name);
            }
        }
        return playingBGM;
    }

    /// <summary>
    /// 未使用のAudioSourceを取得する
    /// </summary>
    private AudioSource GetUnusedAudioSource()
    {
        List<AudioSource> audioSourceList = _audioSourceList.ToList();
        foreach (var item in _bgmDictionary.Values)
        {
            if (item.audioSource != null)
            {
                audioSourceList.Remove(item.audioSource);
            }
        }

        if (audioSourceList.Count > 0)
        {
            return audioSourceList[0];
        }    

        Debug.LogError("未使用のAudioSourceがありません");
        return null;
    }
}