using System;
using UnityEngine;

public class Level : MonoBehaviour
{
    [field: SerializeField] public int level { get; private set; }

    private LevelSO levelSO;
    
    private void Awake()
    {
        levelSO = Resources.Load<LevelSO>("Levels/Level_" + level);
    }

    private void OnMouseDown()
    {
        CampaignManager.Instance.OpenUILevel(this, levelSO);
    }
}