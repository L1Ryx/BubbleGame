using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    int level = 0;

    public void IncreaseLevel()
    {
        level += 1;
    }

    public int GetLevel()
    {
        return level;
    }
}
