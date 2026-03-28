using UnityEngine;
using System.Collections;
using UnityEditor.Rendering.LookDev;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{   
    public static PlayerMovement Instance;
    [SerializeField]private float moveSpeed = 5f;
    [Range(0, 1f)] [SerializeField]private float m_MovementSmoothing = .05f;
    private Rigidbody2D rb;
    private Vector2 movement;
    private Vector3 m_Velocity = Vector3.zero;
    private bool canMove = true;   

    public Animator animator;

    private Vector2 lastDirection;
    void Awake()
    {   
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        lastDirection = Vector2.down;
    }

   void Update()
    {
        if (!canMove)
        {
            movement = Vector2.zero;
            return;
        }

        // Get movement input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement = movement.normalized;

        bool isMoving = movement.sqrMagnitude > 0;
        animator.SetBool("isWalking", isMoving);

        // Determine animation direction
        Vector2 animationDirection = lastDirection;

        if (Input.GetMouseButton(0)) // attacking
        {
            // Face the cursor while attacking
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0f;
            animationDirection = (mousePos - transform.position).normalized;
        }
        else if (isMoving)
        {
            // Face movement direction when moving
            animationDirection = movement;
        }

        // Apply animation direction
        animator.SetFloat("InputX", animationDirection.x);
        animator.SetFloat("InputY", animationDirection.y);

        // Update lastDirection for idle
        if (animationDirection != Vector2.zero)
            lastDirection = animationDirection;

        // Set idle facing
        animator.SetFloat("LastInputX", lastDirection.x);
        animator.SetFloat("LastInputY", lastDirection.y);
    }

    void FixedUpdate()
    {
        if (canMove)
            Move(movement.x, movement.y);
    }

    void Move(float x, float y)
    {
        Vector3 targetVelocity = new Vector2(x * moveSpeed, y * moveSpeed);

        rb.linearVelocity = Vector3.SmoothDamp(
            rb.linearVelocity,
            targetVelocity,
            ref m_Velocity,
            m_MovementSmoothing
        );
    }

    public void EnableMovement()
    {
        canMove = true;
    }

    public void DisableMovement()
    {
        canMove = false;
        rb.linearVelocity = Vector2.zero;   
    }

    public void BoostSpeed(float multiplier, float duration)
    {
        // Stop any previous boost running
        StopCoroutine(nameof(SpeedBoostCoroutine));
        StartCoroutine(SpeedBoostCoroutine(multiplier, duration));
    }

    private IEnumerator SpeedBoostCoroutine(float multiplier, float duration)
    {
        float originalSpeed = moveSpeed;      // save current speed
        moveSpeed *= multiplier;               // apply boost

        yield return new WaitForSeconds(duration);

        moveSpeed = originalSpeed;             // reset speed
    }
}
