using DG.Tweening;
using System;
using UnityEngine;

public class GameState : MonoBehaviour, IInitializable
{
    public static GameState Instance;

    public Action GameReady;
    public Action GameStarted;
    public Action GamePaused;
    public Action GameUnpaused;
    public Action GameFinished;

    public Action ScoreAdded;

    public enum State
    { 
        Ready,
        InGame,
        Paused,
        Finished
    }
    public State CurrentState { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }
    public void Initialize()
    {
        CurrentState = State.Ready;
        GameReady?.Invoke();
    }
    public void StartGame()
    {
        GameStarted?.Invoke();
        CurrentState = State.InGame;
        Time.timeScale = 1.0f;
    }
    public void PauseGame()
    {
        GamePaused?.Invoke();
        CurrentState = State.Paused;
        Time.timeScale = 0.0f;
    }
    public void UnpauseGame()
    {
        GameUnpaused?.Invoke();
        CurrentState = State.InGame;
        Time.timeScale = 1.0f;
    }
    public void FinishGame()
    {
        GameFinished?.Invoke();
        PlayerInput.Instance.IsEnabled = false;
        CurrentState = State.Finished;
        Time.timeScale = 0.0f;

        Camera.main.DOShakePosition(0.5f, 0.3f, fadeOut: true);
        Camera.main.DOShakeRotation(0.5f, 0.3f, fadeOut: true);

        AudioVibrationManager.Instance.PlaySound(AudioVibrationManager.Instance.Win, 1f);
    }
}