using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [Serializable]
    public class PauseUI
    {
        [SerializeField] private GameObject pausePanel;
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button restartButton;

        public void Init()
        {
            SceneC.Instance.GameLoopC.OnPause += OnPause;
            resumeButton.onClick.AddListener(OnResumeButtonClick);
            restartButton.onClick.AddListener(OnRestartButtonClick);
        }

        public void OnPause()
        {
            pausePanel.SetActive(true);
        }

        private void OnResumeButtonClick()
        {
            pausePanel.SetActive(false);
            SceneC.Instance.GameLoopC.Resume();
        }

        private void OnRestartButtonClick()
        {
            SceneC.Instance.GameLoopC.Restart();
        }
    }
}
