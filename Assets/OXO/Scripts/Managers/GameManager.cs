using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Touch;
using Sirenix.OdinInspector;
using UnityEngine;
using ElephantSDK;

public class GameManager : Singleton<GameManager>
{
    [FoldoutGroup("Game Status Bools")]
     public bool isStarted = false;
     [FoldoutGroup("Game Status Bools")]
     public bool isFinished = false;
     [FoldoutGroup("Game Status Bools")]
     public bool isWin = false;
     [FoldoutGroup("Game Status Bools")]
     public bool isFail = false;


    private void Awake()
    {
        Application.targetFrameRate = 60;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    public void StartGame()
    {
        Debug.Log($"<color=#5fe769><b>Game is started!</b> </color>");
        Elephant.LevelStarted(LevelManager.Instance.level);
        CanvasManager.Instance.SetMoney();
        isStarted = true;
        
        isFinished = false;
        isFail = false;
        isWin = false;

        PlayerController.Instance.TapToPlay();
        Actions.OnGameStarted?.Invoke();
    }

    public void FinishTheGame()
    {
        isFinished = true;
        isStarted = false;
    }
    [Button]
    public void LevelComplete()
    {
        isWin = true;
        
        FinishTheGame();
        LeanTouch.Instance.UseMouse = false;
        LeanTouch.Instance.UseTouch = false;
        ConfettiManager.Instance.Play();
        
        Elephant.LevelCompleted(LevelManager.Instance.level);
        CanvasManager.Instance.OpenFinishRect(true);
        
        Actions.OnGameCompleted?.Invoke();
    }
    [Button]
    public void LevelFail()
    {
        isFail = true;
        
        FinishTheGame();

        LeanTouch.Instance.UseMouse = false;
        LeanTouch.Instance.UseTouch = false;
        Elephant.LevelFailed(LevelManager.Instance.level);
        CanvasManager.Instance.OpenFinishRect(false);
        
        Actions.OnGameFailed?.Invoke();
        
    }
}