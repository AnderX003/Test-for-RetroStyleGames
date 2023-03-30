using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [Serializable]
    public class GameOverUI
    {
        [SerializeField] private GameObject GameOverPanel;
        [SerializeField] private Button restartButton;
        [SerializeField] private Text scoreText;

        public void Init()
        {
            SceneC.Instance.GameLoopC.OnGameOver += OnGameOver;
            restartButton.onClick.AddListener(OnRestartButtonClick);
        }

        private void OnGameOver(int score)
        {
            GameOverPanel.SetActive(true);
            scoreText.text = $"Score: {score.ToString()}";
        }

        private void OnRestartButtonClick()
        {
            SceneC.Instance.GameLoopC.Restart();
        }
    }
}
