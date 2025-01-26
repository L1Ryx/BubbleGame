using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    int level = 0;

    [SerializeField]
    private TextMeshProUGUI dialogueText;

    [SerializeField] DialogueCollection InterPlayTexts;
    [SerializeField] Canvas canvas;
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

    public void EnableCanvas()
    {
        canvas.enabled = true;
        if(level >= InterPlayTexts.dialogues.Count)
        {
            canvas.GetComponentInChildren<Button>().gameObject.SetActive(false);
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
