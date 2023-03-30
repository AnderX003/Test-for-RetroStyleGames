using System;
using Helpers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    [Serializable]
    public class GameUI
    {
        public event Action OnDragTriggerDown;
        public event Action OnDragTriggerUp;
        public event Action OnShootButtonClick;
        public event Action OnUltButtonClick;

        [SerializeField] private GameObject gamePanel;
        [SerializeField] private EventTrigger dragTrigger;
        [SerializeField] private Button shootButton;
        [SerializeField] private Button ultButton;
        [SerializeField] private Button pauseButton;
        [SerializeField] private Slider hpSlider;
        [SerializeField] private Slider forceSlider;
        [SerializeField] private Text scoreText;

        [field: SerializeField] public Joystick Joystick { get; private set; }

        public void Init()
        {
            var sceneC = SceneC.Instance;
            var boundsTrigger = dragTrigger;
            boundsTrigger.AddEntry(EventTriggerType.PointerDown, DragTriggerDown);
            boundsTrigger.AddEntry(EventTriggerType.PointerUp, DragTriggerUp);
            shootButton.onClick.AddListener(ShootButtonClick);
            ultButton.onClick.AddListener(UltButtonClick);
            pauseButton.onClick.AddListener(PauseButtonClick);
            var indicators = sceneC.Player.Indicators;
            indicators.OnHpChange += PlayerOnHpChange;
            indicators.OnForceChange += PlayerOnForceChange;
            indicators.OnCanUltStateChanged += OnUltStateChanged;
            OnUltStateChanged(indicators.CanUlt);
            sceneC.GameProgress.OnScoreChanged += OnScoreChanged;
            sceneC.GameLoopC.OnPause += OnPause;
            sceneC.GameLoopC.OnResume += OnResume;
        }

        public void DragTriggerDown(BaseEventData _)
        {
            OnDragTriggerDown?.Invoke();
        }

        public void DragTriggerUp(BaseEventData _)
        {
            OnDragTriggerUp?.Invoke();
        }

        public void ShootButtonClick()
        {
            OnShootButtonClick?.Invoke();
        }

        public void OnUltStateChanged(bool canUlt)
        {
            ultButton.interactable = canUlt;
        }

        private void PauseButtonClick()
        {
            SceneC.Instance.GameLoopC.Pause();
        }

        public void UltButtonClick()
        {
            OnUltButtonClick?.Invoke();
        }

        private void PlayerOnHpChange(int hp, int maxHp)
        {
            hpSlider.value = hp / (float) maxHp;
        }

        private void PlayerOnForceChange(int force, int maxForce)
        {
            forceSlider.value = force / (float) maxForce;
        }

        private void OnScoreChanged(int score)
        {
            scoreText.text = $"Score: {score.ToString()}";
        }

        private void OnPause()
        {
            gamePanel.SetActive(false);
        }

        private void OnResume()
        {
            gamePanel.SetActive(true);
        }
    }
}
