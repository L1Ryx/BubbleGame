using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

public class LightRadiusController : MonoBehaviour
{
    [SerializeField]
    private float startingRadius = 5.0f;

    [SerializeField]
    private float radiusIncrease = 1.0f;

    [SerializeField]
    private float radiusDecay = 0.05f;

    [SerializeField]
    private float lerpDuration = 0.5f; // Duration for the lerp increase

    Light2D spotLight;
    CircleCollider2D lightCollider;

    private Coroutine currentLerpCoroutine; // Track the running coroutine
    private bool isDecreasing = false; // Flag to control radius decay

    public UnityEvent stopPlay; //ik it doesnt fit here but time

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
        if (isDecreasing)
        {
            // Gradually decrease the light radius
            spotLight.pointLightOuterRadius -= (radiusDecay * Time.deltaTime);
            lightCollider.radius -= (radiusDecay * Time.deltaTime);

            // Check if light radius has diminished completely
            if (spotLight.pointLightOuterRadius <= 0)
            {
                stopPlay.Invoke();
            }
        }
    }

    private void IncreaseSize()
    {
        // Stop any ongoing lerp to avoid overlap
        if (currentLerpCoroutine != null)
        {
            StopCoroutine(currentLerpCoroutine);
        }

        // Start a new lerp coroutine
        currentLerpCoroutine = StartCoroutine(LerpRadiusIncrease());
    }

    public void StopRadiusDecay()
    {
        // Stop the radius decay
        isDecreasing = false;
    }

    public void StartRadiusDecay()
    {
        isDecreasing = true;
    }

    private IEnumerator LerpRadiusIncrease()
    {
        float elapsedTime = 0f;
        float startRadius = spotLight.pointLightOuterRadius;
        float targetRadius = startRadius + radiusIncrease;

        float startColliderRadius = lightCollider.radius;
        float targetColliderRadius = startColliderRadius + radiusIncrease;

        while (elapsedTime < lerpDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / lerpDuration;

            // Lerp the radius values
            spotLight.pointLightOuterRadius = Mathf.Lerp(startRadius, targetRadius, t);
            lightCollider.radius = Mathf.Lerp(startColliderRadius, targetColliderRadius, t);

            yield return null;
        }

        // Ensure the final radius is set to the target values
        spotLight.pointLightOuterRadius = targetRadius;
        lightCollider.radius = targetColliderRadius;

        currentLerpCoroutine = null; // Reset coroutine reference
    }
}
