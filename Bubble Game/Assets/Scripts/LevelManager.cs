using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
   [SerializeField] int level = 0;

    [SerializeField]
    private TextMeshProUGUI dialogueText;

    [SerializeField] DialogueCollection InterPlayTexts;
    [SerializeField] Canvas canvas;
    [Header("Data Cubes")]
    public LevelData levelData;

    Dictionary<NPCDataCollection, int> dialogueProgress = new Dictionary<NPCDataCollection, int>();
    private void Start() {
        levelData.ResetLevelCount();
        ShowInterPlayText();
    }

    public int GetDialogueProgress(NPCDataCollection npcType) {
        dialogueProgress.TryAdd(npcType,0);
        return dialogueProgress[npcType];
    }

    public void IncreaseDialogueProgress(NPCDataCollection npcType)
    {
        dialogueProgress[npcType] += 1;
    }

    public void ResetDialogueProgress()
    {
        dialogueProgress.Clear();
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
        Debug.Log("LEVEL INCREASE FUNCTION CALLED");
        level += 1;
        levelData.IncrementLevelCount();
        ResetDialogueProgress();
    }

    public int GetLevel()
    {
        return level;
    }
}
