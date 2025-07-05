using System;
using System.Xml.Serialization;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;

public class StageManager : MonoBehaviour, IInputLockable
{
    [SerializeField] private PlayerManager _playerManager;
    [SerializeField] private SectionManager _sectionManager;
    [SerializeField] private SavePointManager _savePointManager;
    [SerializeField] private StageObjectManager _stageObjectManager;
    [SerializeField] private BackgroundManager _backgroundManager;
    [SerializeField] private CameraManager _cameraManager;
  
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

            await S_TransitionManager.Instance.OutTransition(0.3f, destroyCancellationToken);

            _savePointManager.TeleportSavePoint();
            _playerManager.Initialize(_savePointManager.CurrentSavePoint.IsFacingRight);
            _stageObjectManager.StageObjectInitialize();

            await UniTask.WaitForSeconds(0.05f, cancellationToken: destroyCancellationToken);
            await S_TransitionManager.Instance.InTransition(0.3f, destroyCancellationToken);

            S_InputSystemManager.Instance.SetInputLock(this, false);
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
        _playerManager.EnterDoor();

        await UniTask.WaitForSeconds(0.2f, cancellationToken: destroyCancellationToken);

        UniTask.WaitForSeconds(0.65f, cancellationToken: destroyCancellationToken).ContinueWith(() => S_SEManager.Instance.Play("s_door")).Forget();
        _cameraManager.ChangeCameraKind(CameraKind.Transition);

        await S_FadeManager.Instance.FadeOut(1f, destroyCancellationToken);

        SavePointBase startPoint = _sectionManager.NextSection();
        _savePointManager.TeleportSavePoint(startPoint);
        _playerManager.Initialize(startPoint.IsFacingRight);
        _stageObjectManager.StageObjectInitialize();

        await UniTask.WaitForSeconds(1f, cancellationToken: destroyCancellationToken);
        await S_FadeManager.Instance.FadeIn(1f, destroyCancellationToken);

        S_InputSystemManager.Instance.SetInputLock(this, false);
        _cameraManager.ChangeCameraKind(CameraKind.Main);
        ChangeGameSceneState(GameSceneState.Sleep);
    }

    /// <summary>
    /// ステージをクリアした時の処理
    /// </summary>
    public void StageClear()
    {

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

        ChangeGameSceneState(GameSceneState.Sleep);
        _cameraManager.ChangeCameraKind(CameraKind.Main);

        SavePointBase startPoint = _sectionManager.ChangeSection(0);
        _savePointManager.TeleportSavePoint(startPoint);
        _playerManager.Initialize(startPoint.IsFacingRight);
        _stageObjectManager.StageObjectInitialize();
    }
}
