using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPCDataCollection", menuName = "ScriptableObjects/NPCDataCollection", order = 1)]
public class NPCDataCollection : ScriptableObject
{
    [SerializeField]
    [Tooltip("Wwise Event Name to play for all NPCs in this collection")]
    public string WwiseEventName; // Shared Wwise event name

    [SerializeField]
    public List<NPCDataPerLevel> list;

    [System.Serializable]
    public class NPCDataPerLevel
    {
        [SerializeField]
        [Tooltip("Weight for the chance for this NPC to spawn")]
        public float SpawnRate;

        [SerializeField]
        public DialogueCollection Dialogues;
    }
}
