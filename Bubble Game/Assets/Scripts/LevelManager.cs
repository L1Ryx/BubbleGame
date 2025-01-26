using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    int level = 0;

    [SerializeField]
    private TextMeshProUGUI dialogueText;

    [SerializeField] DialogueCollection InterPlayTexts;

    private void Start() {
        ShowInterPlayText();
    }

    public void ShowInterPlayText()
    {
        if(level >= InterPlayTexts.dialogues.Count)
        {
            dialogueText.text = "The end";
        }
        else
        {
            dialogueText.text = InterPlayTexts.dialogues[level];
        }
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
