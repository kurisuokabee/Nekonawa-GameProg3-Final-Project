using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{   
    [Header("Respawn Settings")]
    [SerializeField] float respawnTime = 3f;
    [Space(10)]
    public int maxHealth = 100;
    int currentHealth;
    Vector2 originalPos;
    Rigidbody2D rb;

    public Transform player;
    PlayerHealth playerHealth;
    [SerializeField] float speed = 5f;

    [SerializeField] Slider healthBar;

    public bool isChasing = false;

    [SerializeField] bool isFirstEnemy;
    [SerializeField] bool isBoss;
    [SerializeField] Animator animator;
    Vector2 idleDirection;
    Vector2 lastAttackDirection = Vector2.down;

    float idleTimer;
    [SerializeField] float idleChangeInterval = 2f;

    float idleDelayTimer;
    [SerializeField] float idleDelayAfterChase = 2f;
    StoryManager storyManager;
    float nextFireTime = 0f;
    float fireRate = 1f;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {   
        currentHealth = maxHealth;
        originalPos = transform.position;
        healthBar.value = maxHealth;

        player = Utilities.Player.transform;
        storyManager = Utilities.StoryManager;
        playerHealth = Utilities.Player.Health;
    }

    void FixedUpdate()
    {   
        if (isBoss)
        {
            BossUpdate();
            return;
        }

        if (isChasing && player != null)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            rb.linearVelocity = direction * speed;

            // Save last attack direction
            lastAttackDirection = direction;

            // Reset idle delay timer
            idleDelayTimer = idleDelayAfterChase;

            // Face player while chasing
            animator.SetBool("isAttacking", true);
            animator.SetFloat("AttackX", direction.x);
            animator.SetFloat("AttackY", direction.y);
            
            // Check if close enough to attack
            if (Utilities.IsEnemyCloseToPlayer(transform))
            {
                if (Time.time < nextFireTime)
                    return;

                playerHealth.TakeDamage(10);
                nextFireTime = Time.time + (1f / fireRate);
            }
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
            animator.SetBool("isAttacking", false);

            // Countdown before switching to random idle
            idleDelayTimer -= Time.fixedDeltaTime;

            if (idleDelayTimer > 0f)
            {
                // Face last attack direction first
                animator.SetFloat("IdleX", lastAttackDirection.x);
                animator.SetFloat("IdleY", lastAttackDirection.y);
            }
            else
            {
                // Then switch to random idle
                idleTimer -= Time.fixedDeltaTime;

                if (idleTimer <= 0f)
                {
                    idleDirection = Random.insideUnitCircle.normalized;
                    idleTimer = idleChangeInterval;
                }

                animator.SetFloat("IdleX", idleDirection.x);
                animator.SetFloat("IdleY", idleDirection.y);
            }
        }
    }

    void BossUpdate()
    {
        if (player == null) return;

        Vector2 direction = (player.position - transform.position).normalized;

        // Just update idle direction to face player
        animator.SetFloat("X", direction.x);
        animator.SetFloat("Y", direction.y);
    }

    //Chases player when triggered
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isChasing = true;
            CancelInvoke(nameof(Respawn));
        }
    }

    //Stop chasing player and respawn
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isChasing = false;
            rb.linearVelocity = Vector2.zero;
            Invoke(nameof(Respawn), respawnTime); // schedule respawn
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.value = currentHealth;

        // Start chasing player when hit
        if (player != null)
        {   
            isChasing = true;
            CancelInvoke(nameof(Respawn));
        }
            

        Debug.Log(gameObject.name + " Health: " + currentHealth);

        if (currentHealth <= 0)
        {   
            if(isBoss)
            {
                storyManager.EnterState(StoryState.Ending);
                speed = 0;
                healthBar.gameObject.SetActive(false);
                rb.linearVelocity = Vector2.zero;
            }
            else if (isFirstEnemy)
            {
                storyManager.EnterState(StoryState.FirstEnemyKilled);
                gameObject.SetActive(false);
            }
            else Die();
        }
    }

    void Die()
    {
        Debug.Log(gameObject.name + " died");
        gameObject.SetActive(false);

        Invoke(nameof(Respawn), respawnTime);
    }

    void Respawn()
    {   
        if(isFirstEnemy || isBoss) return;

        transform.position = originalPos;
        currentHealth = maxHealth;
        healthBar.value = maxHealth;
        isChasing = false;
        gameObject.SetActive(true);
    }
}
