using UnityEngine;
using PlayerDir;
using Bullets;
using Enemies;
using Helpers;
using Pools;
using UI;
using UnityEngine.EventSystems;

public class SceneC : MonoSingleton<SceneC>
{
    [field:SerializeField] public Player Player { get; private set; }
    [field:SerializeField] public UIHolder UIHolder { get; private set; }
    [field:SerializeField] public PoolsHolder PoolsHolder { get; private set; }
    [field:SerializeField] public EnemiesHolder EnemiesHolder { get; private set; }
    public HittablesHolder HittablesHolder { get; private set; }
    public GameProgress GameProgress { get; private set; }
    public GameLoopC GameLoopC { get; private set; }

    private void Start()
    {
        HittablesHolder = new HittablesHolder();
        GameProgress = new GameProgress();
        GameLoopC = new GameLoopC();
        UIHolder.Init();
        HittablesHolder.Init();
        GameProgress.Init();
        GameLoopC.Init();
        PoolsHolder.Init();
        EnemiesHolder.Init();
        Player.Init();
    }
}