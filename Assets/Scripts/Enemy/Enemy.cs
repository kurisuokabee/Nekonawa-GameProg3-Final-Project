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

    Transform player;
    PlayerHealth playerHealth;
    [SerializeField] float speed = 5f;

    [SerializeField] Slider healthBar;

    public bool isChasing = false;

    [SerializeField] bool isFirstEnemy;
    [SerializeField] bool isBoss;
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
        if (isChasing && player != null)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            rb.linearVelocity = direction * speed;

            if(Utilities.IsCloseToPlayer(transform))
            {   
                if (Time.time < nextFireTime)
                return;
                playerHealth.TakeDamage(10);
                nextFireTime = Time.time + (1f / fireRate);
            }
        }
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
