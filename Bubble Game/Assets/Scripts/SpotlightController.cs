using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotlightController : MonoBehaviour
{
    [SerializeField]
    private float startingScale = 5.0f;

    [SerializeField]
    private float scaleIncrease = 1.0f;

    [SerializeField]
    private float scaleDecay = .05f;

    private void OnEnable()
    {
        NPCTrigger.NPCTriggered += IncreaseSize;
    }

    private void OnDisable()
    {
        NPCTrigger.NPCTriggered -= IncreaseSize;
    }

    private void Start()
    {
        this.transform.localScale = new Vector3(startingScale, startingScale, startingScale);
    }

    private void Update()
    {
        Vector3 transform = this.transform.localScale;
        this.transform.localScale = transform - new Vector3(scaleDecay, scaleDecay, scaleDecay);
        if (this.transform.localScale.x <= 0)
        {
            print("You lose and you suck");
        }
    }

    private void IncreaseSize()
    {
        Vector3 transform = this.transform.localScale;
        this.transform.localScale = transform + new Vector3(scaleIncrease, scaleIncrease, scaleIncrease);
    }
}
