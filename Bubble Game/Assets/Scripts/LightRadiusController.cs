using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightRadiusController : MonoBehaviour
{
    [SerializeField]
    private float startingRadius = 5.0f;

    [SerializeField]
    private float radiusIncrease = 1.0f;

    [SerializeField]
    private float radiusDecay = .05f;

    Light2D spotLight;
    CircleCollider2D lightCollider;

    private void OnEnable()
    {
        NPCTrigger.NPCTriggered += IncreaseSize;
    }

    private void OnDisable()
    {
        NPCTrigger.NPCTriggered -= IncreaseSize;
    }

    private void Awake()
    {
        spotLight = GetComponentInParent<Light2D>();
        lightCollider = GetComponentInParent<CircleCollider2D>();
    }

    private void Start()
    {
        spotLight.pointLightOuterRadius = startingRadius;
        lightCollider.radius = startingRadius;
    }

    private void Update()
    {
        spotLight.pointLightOuterRadius -= (radiusDecay*Time.deltaTime);
        lightCollider.radius -= (radiusDecay * Time.deltaTime);
        if (spotLight.pointLightOuterRadius <= 0)
        {
            print("You lose and you suck");
            Time.timeScale = 0;
        }
    }

    private void IncreaseSize()
    {
        spotLight.pointLightOuterRadius += radiusIncrease;
        lightCollider.radius += radiusIncrease;
    }
}
