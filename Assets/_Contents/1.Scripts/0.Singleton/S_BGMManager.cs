using UnityEngine;
using System;
using System.Linq;
using System.Threading;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public class S_BGMManager : Singleton<S_BGMManager>
{
    enum BGMStatus {  none, fadeIn, play, fadeOut, pause, stop }

    [Serializable] class BGMInfo
    {
        [SerializeField] public string name;
        [SerializeField] public AudioClip[] audioClip;

        [HideInInspector] public AudioSource audioSource;
        [HideInInspector] public CancellationTokenSource cancellationTokenSource;
        [HideInInspector] public BGMStatus status = BGMStatus.none;
        private int _clipIndex = 0;
        public int clipIndex
        {
            get => _clipIndex;
            set
            {
                _clipIndex = value;
                if (_clipIndex >= audioClip.Length) _clipIndex = 0;

                if (audioSource != null) audioSource.clip = audioClip[_clipIndex];
            }
        }

        public bool IsFinished()
        {
            return audioSource.time == 0.0f && !audioSource.isPlaying;
        }
    }

    [SerializeField] float MAX_VOLUME;
    [SerializeField] List<BGMInfo> BGMList = new List<BGMInfo>();

    private AudioSource[] _audioSourceList = new AudioSource[5];
    private Dictionary<string, BGMInfo> _BGMDictionary = new Dictionary<string, BGMInfo>();
    private float _volume;
 
    public override void Awake()
    {
        base.Awake();

        for (int i = 0; i < BGMList.Count; ++i) _BGMDictionary.Add(BGMList[i].name, BGMList[i]);
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
        foreach (var item in _BGMDictionary.Values)
        {
            if (item.audioSource == null) continue;
            if ( (item.status == BGMStatus.fadeIn || item.status == BGMStatus.play || item.status == BGMStatus.fadeOut) && item.IsFinished())
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
        if (playingBGM.Any(x => x == name)) return;
        
        if (_BGMDictionary.TryGetValue(name, out var BGMInfo))
        {
            if (BGMInfo.cancellationTokenSource != null) BGMInfo.cancellationTokenSource.Cancel();
            BGMInfo.cancellationTokenSource = new CancellationTokenSource();

            if (BGMInfo.status == BGMStatus.fadeOut || BGMInfo.status == BGMStatus.pause) {
                ResumeAsync(BGMInfo, fadeTime, BGMInfo.cancellationTokenSource.Token);
            }
            else if (BGMInfo.status == BGMStatus.stop || BGMInfo.status == BGMStatus.none) {
                BGMInfo.audioSource = GetUnusedAudioSource();
                if (BGMInfo.audioSource == null) return;

                BGMInfo.clipIndex = BGMInfo.clipIndex;
                PlayAsync(BGMInfo, fadeTime, BGMInfo.cancellationTokenSource.Token);
            }
        }
    }
    private async void PlayAsync(BGMInfo BGMInfo, float fadeTime, CancellationToken token)
    {
        BGMInfo.status = BGMStatus.fadeIn;
        BGMInfo.audioSource.Play();
        float currentVolume = BGMInfo.audioSource.volume;

        for (int i = 0; i < 100; i++)
        {
            BGMInfo.audioSource.volume += ( _volume - currentVolume ) / 100;
            if (BGMInfo.audioSource.volume > _volume) BGMInfo.audioSource.volume = _volume;

            try { await UniTask.Delay(TimeSpan.FromSeconds(fadeTime / 100), cancellationToken: token); }
            catch { return; }
        }
        BGMInfo.audioSource.volume = _volume;
        BGMInfo.cancellationTokenSource = null;
        BGMInfo.status = BGMStatus.play;
    }
    private async void ResumeAsync(BGMInfo BGMInfo, float fadeTime, CancellationToken token)
    {
        BGMInfo.status = BGMStatus.fadeIn;
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
        BGMInfo.status = BGMStatus.play;
    }

    /// <summary>
    /// BGMを停止する
    /// </summary>
    /// <param name="name">BGMの名前</param>
    /// <param name="fadeTime">フェードアウトの時間</param>
    public void Stop(string name, float fadeTime)
    {
        if (_BGMDictionary.TryGetValue(name, out var BGMInfo))
        {
            if (BGMInfo.cancellationTokenSource != null) BGMInfo.cancellationTokenSource.Cancel();
            BGMInfo.cancellationTokenSource = new CancellationTokenSource();

            StopAsync(BGMInfo, fadeTime, BGMInfo.cancellationTokenSource.Token);
        }
    }
    private async void StopAsync(BGMInfo BGMInfo, float fadeTime, CancellationToken token)
    {
        BGMInfo.status = BGMStatus.fadeOut;
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
        BGMInfo.status = BGMStatus.stop;
    }

    /// <summary>
    /// BGMを一時停止する
    /// </summary>
    /// <param name="name">BGMの名前</param>
    /// <param name="fadeTime">フェードアウトの時間</param>
    public void Pause(string name, float fadeTime)
    {
        if (_BGMDictionary.TryGetValue(name, out var BGMInfo))
        {
            if (BGMInfo.cancellationTokenSource != null) BGMInfo.cancellationTokenSource.Cancel();
            BGMInfo.cancellationTokenSource = new CancellationTokenSource();

            PauseAsync(BGMInfo, fadeTime, BGMInfo.cancellationTokenSource.Token);
        }
    }
    private async void PauseAsync(BGMInfo BGMInfo, float fadeTime, CancellationToken token)
    {
        BGMInfo.status = BGMStatus.fadeOut;
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
        BGMInfo.status = BGMStatus.pause;
    }

    /// <summary>
    /// BGMをミュートする
    /// </summary>
    public void Mute()
    {
        foreach (var item in _BGMDictionary.Values)
        {
            item.audioSource.pitch = 0;
        }
    }
    /// <summary>
    /// BGMのミュートを解除する
    /// </summary>
    public void UnMute()
    {
        foreach (var item in _BGMDictionary.Values)
        {
            item.audioSource.pitch = 1;
        }
    }

    /// <summary>
    /// BGMの音量を変更する
    /// </summary>
    public void ChangeVolume(float volume)
    {
        _volume = volume * MAX_VOLUME;
        foreach (var item in _BGMDictionary.Values)
        {
            if (item.status == BGMStatus.fadeIn || item.status == BGMStatus.play) item.audioSource.volume = _volume;
        }
    }

    /// <summary>
    /// 再生中のBGMを取得する
    /// </summary>
    private List<string> GetPlayingBGM()
    {
        List<string> playingBGM = new List<string>();
        foreach (var item in _BGMDictionary.Values)
        {
            if (item.status == BGMStatus.fadeIn || item.status == BGMStatus.play) playingBGM.Add(item.name);
        }
        return playingBGM;
    }

    /// <summary>
    /// 未使用のAudioSourceを取得する
    /// </summary>
    private AudioSource GetUnusedAudioSource()
    {
        List<AudioSource> audioSourceList = _audioSourceList.ToList();
        foreach (var item in _BGMDictionary.Values)
        {
            if (item.audioSource != null) audioSourceList.Remove(item.audioSource);
        }
        if (audioSourceList.Count > 0) return audioSourceList[0];

        Debug.LogError("未使用のAudioSourceがありません");
        return null;
    }
}