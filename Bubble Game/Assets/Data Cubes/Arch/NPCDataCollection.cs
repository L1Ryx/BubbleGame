using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPCDataCollection", menuName = "ScriptableObjects/NPCDataCollection", order = 1)]
public class NPCDataCollection : ScriptableObject
{
    [SerializeField]
    public List<NPCDataPerLevel> list;

    [System.Serializable]
    public class NPCDataPerLevel{
        [SerializeField]
        [Tooltip("Weight for the chance for this npc to spawn")]
        public float SpawnRate;
    
        [SerializeField]
        public DialogueCollection Dialogues;
    }
}
