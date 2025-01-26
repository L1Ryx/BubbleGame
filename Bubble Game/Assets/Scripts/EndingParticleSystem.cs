using System.Collections;
using UnityEngine;

public class EndingParticleSystem : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem particleSystem; // Reference to the particle system

    [SerializeField]
    private int minEmission = 50; // Default minimum emission rate

    [SerializeField]
    private int maxEmission = 500; // Default maximum emission rate

    [SerializeField]
    private float increaseSpeed = 2.0f; // Speed at which the emission increases

    [SerializeField]
    private float maxEmissionDuration = 5.0f; // Time to stay at max emission

    private ParticleSystem.EmissionModule emissionModule;

    private void Awake()
    {
        if (particleSystem == null)
        {
            particleSystem = GetComponent<ParticleSystem>();
        }

        if (particleSystem == null)
        {
            Debug.LogError("ParticleSystem is not assigned or found on the GameObject.");
            return;
        }

        // Get the EmissionModule of the particle system
        emissionModule = particleSystem.emission;

        // Disable the particle system at the start
        particleSystem.Stop();
        emissionModule.rateOverTime = minEmission; // Start with min emission
    }

    public void TriggerEndingParticles()
    {
        StartCoroutine(PlayParticles());
    }

    private IEnumerator PlayParticles()
    {
        // Enable the particle system and start at min emission
        particleSystem.Play();
        emissionModule.rateOverTime = minEmission;

        // Gradually increase the emission rate to the max
        float currentEmission = minEmission;
        while (currentEmission < maxEmission)
        {
            currentEmission += increaseSpeed * Time.deltaTime;
            emissionModule.rateOverTime = currentEmission;
            yield return null;
        }

        // Ensure the emission rate is exactly the max value
        emissionModule.rateOverTime = maxEmission;

        // Wait for the max emission duration
        yield return new WaitForSeconds(maxEmissionDuration);

        // Turn off the particle system
        particleSystem.Stop();
    }
}
