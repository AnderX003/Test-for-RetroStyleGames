using System;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameLoopC
{
    public event Action OnPause;
    public event Action OnResume;
    public event Action<int> OnGameOver;

    public void Init()
    {
        Time.timeScale = 1f;
        SceneC.Instance.Player.OnPlayerKilled += GameOver;
    }

    private void GameOver()
    {
        Time.timeScale = 0f;
        OnGameOver?.Invoke(SceneC.Instance.GameProgress.Score);
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        OnPause?.Invoke();
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        OnResume?.Invoke();
    }
}
