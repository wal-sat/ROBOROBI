using System;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private SavePointManager savePointManager;
    [SerializeField] private StageObjectManager stageObjectManager;
    [SerializeField] private BackgroundManager backgroundManager;
    [SerializeField] private CameraManager cameraManager;
  
    public Action<GameSceneState> ChangeGameSceneState;

    private SceneKind _currentSceneKind;

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
    public void PlayerActivate()
    {

    }

    /// <summary>
    /// Playerがやられた時の処理
    /// </summary>
    public void PlayerDeath()
    {
        Debug.Log("death");
    }

    /// <summary>
    /// Playerがドアに入った時の処理を
    /// </summary>
    public void PlayerEnterDoor()
    {

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
    }
}
