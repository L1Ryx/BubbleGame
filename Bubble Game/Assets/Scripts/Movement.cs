using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField]
    private float normalSpeed = 5.0f;

    [SerializeField]
    private float slowSpeed = 2.0f;

    [SerializeField]
    private float fadeDuration = 2.0f; // Default fade-out duration in seconds

    [SerializeField]
    private Animator animator;

    private SpriteRenderer spriteRenderer;
    private float speed;
    private bool isInputDisabled = true; // Flag to disable input

    void Start()
    {
        speed = normalSpeed;

        // Get the SpriteRenderer component
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("Movement requires a SpriteRenderer to fade out.");
        }
    }

    void Update()
    {
        if (isInputDisabled) return; // Disable input when the flag is set

        // Get input
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        // Determine movement direction
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0).normalized;

        // Move the character
        transform.Translate(direction * speed * Time.deltaTime);

        HandleMovementAnim(horizontalInput, verticalInput, direction);
    }

    private void HandleMovementAnim(float horizontalInput, float verticalInput, Vector3 direction)
    {
        // Check if player is moving
        bool isMoving = direction.magnitude > 0;

        // Update "IsMoving" Animator parameter
        animator.SetBool("IsMoving", isMoving);

        if (isMoving)
        {
            // Prioritize horizontal movement over vertical if both inputs are active
            if (Mathf.Abs(horizontalInput) >= Mathf.Abs(verticalInput))
            {
                animator.SetFloat("Horizontal", horizontalInput);
                animator.SetFloat("Vertical", 0); // Ignore vertical input
            }
            else
            {
                animator.SetFloat("Horizontal", 0); // Ignore horizontal input
                animator.SetFloat("Vertical", verticalInput);
            }
        }
        else
        {
            // Stop movement (reset parameters to idle state)
            animator.SetFloat("Horizontal", 0);
            animator.SetFloat("Vertical", 0);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "light")
        {
            speed = slowSpeed;
            AkSoundEngine.SetRTPCValue("Brightness", 0);

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "light")
        {
            speed = normalSpeed;
            AkSoundEngine.SetRTPCValue("Brightness", 100);
        }
    }

    public void FadeOutAndDisableInput()
    {
        // Disable input
        isInputDisabled = true;

        // Reset movement animations to idle
        animator.SetFloat("Horizontal", 0);
        animator.SetFloat("Vertical", 0);
        animator.SetBool("IsMoving", false);

        // Start the fade-out coroutine
        StartCoroutine(FadeOutCoroutine());
    }

    public void EnableMovement()
    {
        // enable input
        isInputDisabled = false;
    }

    public void ResetPlayer()
    {
        gameObject.transform.position = Vector3.zero;
        spriteRenderer.color = Color.black;
    }


    private IEnumerator FadeOutCoroutine()
    {
        float elapsedTime = 0f;
        Color originalColor = spriteRenderer.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1.0f, 0.0f, elapsedTime / fadeDuration);
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        // Ensure the player is fully transparent
        spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.0f);

        // You can add additional logic here (e.g., disable the GameObject)
        //gameObject.SetActive(false);
    }
}
