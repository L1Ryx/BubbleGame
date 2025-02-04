using System;
using System.Collections;
using UnityEngine;
using TMPro;

public class NPCTrigger : MonoBehaviour
{
    public static Action NPCTriggered = delegate { };

    [SerializeField]
    private float destructionDelay = 2.0f;

    [SerializeField]
    private float letterDisplayInterval = 0.05f;

    [SerializeField]
    private float fadeInSpeed = 2.0f;

    [SerializeField]
    private NPCDataCollection npcDataCollection; // Reference to the ScriptableObject

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private AnimatorOverrideController childAnimations; // Child animation override
    [SerializeField]
    private AnimatorOverrideController adultAnimations; // Adult animation override

    private SpriteRenderer spriteRenderer;
    private Canvas dialogueCanvas;
    private TextMeshProUGUI dialogueText;
    private bool isFadingOut = false;

    private LevelManager levelManager;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("NPCTrigger requires a SpriteRenderer to fade out.");
        }

        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            spriteRenderer.color = new Color(color.r, color.g, color.b, 0f);
        }

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
        dialogueCanvas.enabled = false;
        dialogueText.text = string.Empty;
    }

    private void Start()
    {
        StartCoroutine(FadeIn());
        

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !isFadingOut)
        {
            isFadingOut = true;
            NPCTriggered.Invoke();

            // Play the Wwise event
            PlayWwiseEvent();

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

        spriteRenderer.color = new Color(color.r, color.g, color.b, 1f);
    }

    private void PlayWwiseEvent()
    {
        if (npcDataCollection == null)
        {
            Debug.LogError("NPCDataCollection is not assigned in NPCTrigger.");
            return;
        }

        if (string.IsNullOrEmpty(npcDataCollection.WwiseEventName))
        {
            Debug.LogError("WwiseEventName is empty or not assigned in the NPCDataCollection.");
            return;
        }

        // Post the Wwise event
        AkSoundEngine.PostEvent(npcDataCollection.WwiseEventName, gameObject);
        Debug.Log($"Played Wwise event: {npcDataCollection.WwiseEventName}");
    }

    private IEnumerator DisplayDialogue()
    {
        dialogueCanvas.enabled = true;
        string dialogue = GetComponentInParent<NPCData>().GetDialogue(levelManager.GetLevel(), levelManager.GetDialogueProgress(npcDataCollection));

        dialogueText.text = string.Empty;
        foreach (char letter in dialogue)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(letterDisplayInterval);
        }
        levelManager.IncreaseDialogueProgress(npcDataCollection);
    }

    public void FadeOutAndDestroy()
    {
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

        spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.0f);
        Destroy(gameObject);
    }

    // Function to switch from child animations to adult animations
    public void SwitchToAdultAnimations()
    {
        if (animator.runtimeAnimatorController == childAnimations)
        {
            animator.runtimeAnimatorController = adultAnimations;
            
        }
    }

    public void SetChildAnimations()
    {
        if (animator != null && childAnimations != null)
        {
            animator.runtimeAnimatorController = childAnimations;
            
        }
    }
}
