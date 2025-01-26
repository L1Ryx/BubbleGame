using System.Collections;
using UnityEngine;

public class Background : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer

    [SerializeField]
    private float lerpSpeed = 2.0f; // Speed of the color transition

    private Coroutine currentCoroutine; // Track the active coroutine

    private void Awake()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer is not assigned or found on the GameObject.");
        }
    }

    public void LerpToBlack(float delay = 0f)
    {
        // Start the transition to black after the delay
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        currentCoroutine = StartCoroutine(LerpColorWithDelay(Color.black, delay));
    }

    public void LerpToOriginalColor(Color originalColor, float delay = 0f)
    {
        // Start the transition to the original color after the delay
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        currentCoroutine = StartCoroutine(LerpColorWithDelay(originalColor, delay));
    }

    private IEnumerator LerpColorWithDelay(Color targetColor, float delay)
    {
        // Wait for the delay before starting the transition
        yield return new WaitForSeconds(delay);

        Color startColor = spriteRenderer.color;
        float elapsedTime = 0f;

        while (elapsedTime < lerpSpeed)
        {
            elapsedTime += Time.deltaTime;
            spriteRenderer.color = Color.Lerp(startColor, targetColor, elapsedTime / lerpSpeed);
            yield return null;
        }

        // Ensure the final color is exactly the target color
        spriteRenderer.color = targetColor;
        currentCoroutine = null; // Clear the active coroutine reference
    }
}
