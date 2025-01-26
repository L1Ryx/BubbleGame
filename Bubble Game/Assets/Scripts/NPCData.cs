using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCData : MonoBehaviour
{
    [SerializeField] NPCDataCollection nPCData;

    int level = 0;

    public string GetRandomDialogue()
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

    public float GetSpawnRate()
    {
        return nPCData.list[level].SpawnRate;
    }


}
