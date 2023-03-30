using UnityEngine;

namespace UI
{
    public class UIHolder : MonoBehaviour
    {
        [field:SerializeField] public GameUI GameUI { get; private set; }
        [field:SerializeField] public PauseUI PauseUI { get; private set; }
        [field:SerializeField] public GameOverUI GameOverUI { get; private set; }

        public void Init()
        {
            GameUI.Init();
            PauseUI.Init();
            GameOverUI.Init();
        }
    }
}
