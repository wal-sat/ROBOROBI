using System;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;
using DG.Tweening;

public class StageManager : MonoBehaviour, IInputLockable
{
    [SerializeField] private PlayerManager _playerManager;
    [SerializeField] private SectionManager _sectionManager;
    [SerializeField] private SavePointManager _savePointManager;
    [SerializeField] private GearManager _gearManager;
    [SerializeField] private StageObjectManager _stageObjectManager;
    [SerializeField] private BackgroundManager _backgroundManager;
    [SerializeField] private CameraManager _cameraManager;

    [SerializeField] private DeathCountManager _deathCountManager;
    [SerializeField] private PlayTimeManager _playTimeManager;
    [SerializeField] private GameSceneUIManager _gameSceneUIManager;
  
    public Action<GameSceneState> ChangeGameSceneState;

    private SceneKind _currentSceneKind;
    private FirstCallChecker _deathFirstCallChecker = new FirstCallChecker();

    // ----- Life Cycle Methods -----

    private void Awake()
    {
        _currentSceneKind = S_LoadSceneManager.Instance.GetCurrentSceneKind();
    }

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        _gameSceneUIManager.UpdatePlayTime( _playTimeManager.GetPlayTimeString() );
    }

    // ----- Public Methods -----

    /// <summary>
    /// Sleep状態からPlaying状態への遷移する時の処理
    /// </summary>
    [Button]
    public void PlayerActivate()
    {
        ChangeGameSceneState(GameSceneState.Playing);
        _cameraManager.ChangeCameraKind(CameraKind.Main);
        _gameSceneUIManager.DisplayUI(GameSceneUIState.Playing);

        _playerManager.Activate(_savePointManager.CurrentSavePoint.AcquiredActionData);
    }

    /// <summary>
    /// Playerがやられた時の処理
    /// </summary>
    public async UniTaskVoid PlayerDeath(float angleZ = 0)
    {
        if (_deathFirstCallChecker.Check())
        {
            S_InputSystemManager.Instance.SetInputLock(this, true);

            _playerManager.Death(angleZ);
            _cameraManager.ChangeCameraKind(CameraKind.Transition);

            await UniTask.WaitForSeconds(0.95f, cancellationToken: destroyCancellationToken);

            S_SEManager.Instance.Play("u_restartTransition");

            await S_TransitionManager.Instance.OutTransition(0.3f, destroyCancellationToken);

            _savePointManager.TeleportSavePoint();
            _playerManager.Initialize(_savePointManager.CurrentSavePoint.IsFacingRight);
            _gearManager.GearInitialize();
            _stageObjectManager.StageObjectInitialize();
            _backgroundManager.BackgroundInitialize();
            _deathCountManager.IncrementDeathCount();

            _gameSceneUIManager.ChangeDeathCount( _deathCountManager.DeathCount );
            _gameSceneUIManager.DisplayUI(GameSceneUIState.Sleep);

            S_InputSystemManager.Instance.SetInputLock(this, false);
            
            await UniTask.WaitForSeconds(0.05f, cancellationToken: destroyCancellationToken);
            await S_TransitionManager.Instance.InTransition(0.3f, destroyCancellationToken);

            ChangeGameSceneState(GameSceneState.Sleep);

            _deathFirstCallChecker.Reset();
        }
    }

    /// <summary>
    /// Playerがドアに入った時の処理を
    /// </summary>
    public async UniTaskVoid PlayerEnterDoor()
    {
        S_InputSystemManager.Instance.SetInputLock(this, true);
        _playerManager.SectionClear();
        _gearManager.OnSave();

        await UniTask.WaitForSeconds(0.2f, cancellationToken: destroyCancellationToken);

        UniTask.WaitForSeconds(0.65f, cancellationToken: destroyCancellationToken).ContinueWith(() => S_SEManager.Instance.Play("s_door")).Forget();
        _cameraManager.ChangeCameraKind(CameraKind.Transition);

        await S_FadeManager.Instance.FadeOut(1f, destroyCancellationToken);

        SavePointBase startPoint = _sectionManager.NextSection();
        _savePointManager.TeleportSavePoint(startPoint);
        _playerManager.Initialize(startPoint.IsFacingRight);
        _gearManager.GearInitialize();
        _stageObjectManager.StageObjectInitialize();
        _backgroundManager.BackgroundInitialize();

        _gameSceneUIManager.DisplayUI(GameSceneUIState.Sleep);

        await UniTask.WaitForSeconds(1f, cancellationToken: destroyCancellationToken);
        await S_FadeManager.Instance.FadeIn(1f, destroyCancellationToken);

        S_InputSystemManager.Instance.SetInputLock(this, false);
        _cameraManager.ChangeCameraKind(CameraKind.Main);
        ChangeGameSceneState(GameSceneState.Sleep);
    }

    /// <summary>
    /// ステージをクリアした時の処理
    /// </summary>
    public async UniTaskVoid StageClear(ClearKind clearKind)
    {
        S_InputSystemManager.Instance.SetInputLock(this, true);

        DOVirtual.Float(1f, 0f, 0.5f, value => Time.timeScale = value).SetEase(Ease.OutCubic).SetUpdate(true);

        _playTimeManager.StopTimer();
        _gearManager.OnSave();
        S_StageInfoManager.Instance.AddDeathCount(_currentSceneKind, _deathCountManager.DeathCount, true);
        S_StageInfoManager.Instance.AddPlayTime(_currentSceneKind, _playTimeManager.GetPlayTimeInt(), true);
        _gameSceneUIManager.ChangeClearPanelInformation(_currentSceneKind, clearKind, _deathCountManager.DeathCount.ToString(), _playTimeManager.GetPlayTimeString());

        await UniTask.WaitForSeconds(0.5f, true);

        _playerManager.SectionClear();
        _gameSceneUIManager.DisplayUI(GameSceneUIState.Clear);
        ChangeGameSceneState(GameSceneState.Clear);

        await UniTask.WaitForSeconds(0.5f, true);

        S_InputSystemManager.Instance.SetInputLock(this, false);

    }

    /// <summary>
    /// ボーズ時の処理
    /// </summary>
    public void PauseGame()
    {

    }
    public void UnPauseGame()
    {

    }

    // ----- Private Methods -----

    private void Initialize()
    {
        Time.timeScale = 1;
        S_BGMManager.Instance.Play("stage", 1.5f);

        SavePointBase startPoint = _sectionManager.ChangeSection(0);
        _savePointManager.TeleportSavePoint(startPoint);
        _playerManager.Initialize(startPoint.IsFacingRight);
        _gearManager.GearInitialize();
        _stageObjectManager.StageObjectInitialize();
        _backgroundManager.BackgroundInitialize();

        ChangeGameSceneState(GameSceneState.Sleep);
        _cameraManager.ChangeCameraKind(CameraKind.Main);

        _deathCountManager.ResetDeathCount();
        _playTimeManager.ResetTimer();
        _playTimeManager.StartTimer();

        _gameSceneUIManager.ChangeStageName(_currentSceneKind);
        _gameSceneUIManager.ChangeDeathCount(_deathCountManager.DeathCount);
        _gameSceneUIManager.DisplayUI(GameSceneUIState.Sleep);
    }
}
