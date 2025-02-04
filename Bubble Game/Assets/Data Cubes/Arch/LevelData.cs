using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData", order = 1)]
public class LevelData : ScriptableObject
{
    [Header("Level Info")]
    public int levelCount = 0;
    public void ResetLevelCount() {
        levelCount = 0;
    }

    public void IncrementLevelCount() {
        levelCount++;
    }

    public int GetLevelCount() {
        return levelCount;
    }
}
