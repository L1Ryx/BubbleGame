using System;
using System.Collections;
using UnityEngine;
using TMPro;

/*
* Hi Brett sorry for cramping so much non NPC Trigger logic into here lol. Took me too long
* to realize this script was just for the trigger. Will refactor later.
*/
public class NPCTrigger : MonoBehaviour
{
    public static Action NPCTriggered = delegate { };

    [SerializeField]
    private float destructionDelay = 2.0f; // Time in seconds before the NPC is destroyed

    [SerializeField]
    private float letterDisplayInterval = 0.05f; // Time between letters being displayed

    [SerializeField]
    private float fadeInSpeed = 2.0f; // Speed for NPC fade-in

    [SerializeField]
    private DialogueCollection dialogueCollection; // ScriptableObject containing dialogues

    private SpriteRenderer spriteRenderer;
    private Canvas dialogueCanvas;
    private TextMeshProUGUI dialogueText;
    private bool isFadingOut = false; // Flag to prevent retriggering during fade-out

    private LevelManager levelManager;

    private void Awake()
    {
        // Get the SpriteRenderer
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("NPCTrigger requires a SpriteRenderer to fade out.");
        }

        // Set initial alpha to 0
        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            spriteRenderer.color = new Color(color.r, color.g, color.b, 0f);
        }

        // Get the dialogue canvas and TMP Text
        dialogueCanvas = GetComponentInChildren<Canvas>();
        if (dialogueCanvas == null)
        {
            Debug.LogError("NPCTrigger requires a child Canvas for dialogue.");
        }

        dialogueText = dialogueCanvas.GetComponentInChildren<TextMeshProUGUI>();
        if (dialogueText == null)
        {
            Debug.LogError("NPCTrigger requires a child TextMeshProUGUI for dialogue.");
        }

        levelManager = FindObjectsByType<LevelManager>(FindObjectsSortMode.None)[0];

        // Ensure the dialogue text is initially invisible
        dialogueCanvas.enabled = false;
        dialogueText.text = string.Empty;
    }

    private void Start()
    {
        // Start fade-in when NPC spawns
        StartCoroutine(FadeIn());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !isFadingOut) // Prevent retriggering
        {
            isFadingOut = true; // Set the flag
            NPCTriggered.Invoke();
            StartCoroutine(DisplayDialogue());
            StartCoroutine(FadeAndDestroy());
        }
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

        // Ensure the sprite is fully opaque
        spriteRenderer.color = new Color(color.r, color.g, color.b, 1f);
    }

    private IEnumerator DisplayDialogue()
    {
        // Ensure the canvas is visible
        dialogueCanvas.enabled = true;

        // Choose a random dialogue from the collection
        string dialogue = GetComponentInParent<NPCData>().GetRandomDialogue(levelManager.GetLevel());

        // Display the dialogue letter by letter
        dialogueText.text = string.Empty;
        foreach (char letter in dialogue)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(letterDisplayInterval);
        }
    }

    public void FadeOutAndDestroy() {
        StartCoroutine(FadeAndDestroy());
    }

    private IEnumerator FadeAndDestroy()
    {
        float elapsedTime = 0f;
        Color originalColor = spriteRenderer.color;

        while (elapsedTime < destructionDelay)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1.0f, 0.0f, elapsedTime / destructionDelay);
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        // Ensure the sprite is fully transparent
        spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.0f);

        // Destroy the GameObject
        Destroy(gameObject);
    }
}
