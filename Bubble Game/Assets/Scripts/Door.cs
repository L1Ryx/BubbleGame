using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Door : MonoBehaviour
{
    [SerializeField]
    private float fadeInSpeed = 2.0f; // Time to fade in

    [SerializeField]
    private float fadeOutSpeed = 2.0f; // Time to fade out

    [SerializeField]
    private float lifetime = 10.0f; // Time before the door starts fading out automatically

    private SpriteRenderer spriteRenderer;
    private bool isFading = false; // Flag to prevent multiple fade operations
    [Header("Events")]
    public UnityEvent stopPlay;

    private void Awake()
    {
        // Get the SpriteRenderer component
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("Door requires a SpriteRenderer component to fade.");
        }

        // Set the initial alpha to 0 (invisible)
        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            spriteRenderer.color = new Color(color.r, color.g, color.b, 0f);
        }
    }

    private void Start()
    {
        // Start the fade-in process
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        Color color = spriteRenderer.color;

        while (elapsedTime < fadeInSpeed)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeInSpeed);
            spriteRenderer.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        // Ensure the door is fully visible
        spriteRenderer.color = new Color(color.r, color.g, color.b, 1f);

        // Wait for the lifetime before starting the fade-out
        yield return new WaitForSeconds(lifetime);

        // Automatically fade out after the lifetime
        if (!isFading)
        {
            StartCoroutine(FadeOut(fadeOutSpeed));
        }
    }

    private IEnumerator FadeOut(float fadeDuration)
    {
        isFading = true;
        float elapsedTime = 0f;
        Color color = spriteRenderer.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            spriteRenderer.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        // Ensure the door is fully transparent and destroy it
        spriteRenderer.color = new Color(color.r, color.g, color.b, 0f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !isFading)
        {
            // If the player touches the door, fade out immediately
            stopPlay.Invoke();
            StartCoroutine(FadeOut(fadeOutSpeed));
        }
    }
}
