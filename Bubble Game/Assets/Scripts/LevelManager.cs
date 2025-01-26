using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    int level = 0;

    [SerializeField]
    private TextMeshProUGUI dialogueText;

    public void ShowInterPlayText()
    {
        dialogueText.text = "Level " + level.ToString();
    }

    public void IncreaseLevel()
    {
        level += 1;
    }

    public int GetLevel()
    {
        return level;
    }
}
