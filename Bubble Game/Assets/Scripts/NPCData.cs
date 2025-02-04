using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCData : MonoBehaviour
{
    [SerializeField] NPCDataCollection nPCData;

    public string GetDialogue(int level, int dialogueProgress)
    {
        DialogueCollection dialogueCollection = nPCData.list[level].Dialogues;
        if(dialogueProgress >= dialogueCollection.dialogues.Count)
        {
            return GetRandomDialogue(level);
        }
        return nPCData.list[level].Dialogues.dialogues[dialogueProgress];
    }

    public string GetRandomDialogue(int level)
    {
        DialogueCollection dialogueCollection = nPCData.list[level].Dialogues;

        if (dialogueCollection == null || dialogueCollection.dialogues.Count == 0)
        {
            Debug.LogWarning("DialogueCollection is empty or not assigned!");
            return "No dialogue available.";
        }

        int randomIndex = UnityEngine.Random.Range(0, dialogueCollection.dialogues.Count);
        return dialogueCollection.dialogues[randomIndex];
    }

    public float GetSpawnRate(int level)
    {
        return nPCData.list[level].SpawnRate;    
    }


}
