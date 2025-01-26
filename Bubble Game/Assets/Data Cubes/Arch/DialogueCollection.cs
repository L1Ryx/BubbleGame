using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueCollection", menuName = "ScriptableObjects/DialogueCollection", order = 1)]
public class DialogueCollection : ScriptableObject
{
    [TextArea(2, 5)] 
    public List<string> dialogues = new List<string>();
}
