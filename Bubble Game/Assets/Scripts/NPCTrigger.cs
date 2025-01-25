using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTrigger : MonoBehaviour
{
    public static Action NPCTriggered = delegate { };
    private void OnCollisionEnter2D(Collision2D collision)
    {
        print("skibidi rizzler");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("skibidi rizzler");
        NPCTriggered.Invoke();
    }
}
