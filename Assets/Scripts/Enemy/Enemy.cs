using UnityEngine;

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
    [SerializeField] float speed = 5f;

    bool isChasing = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {   
        currentHealth = maxHealth;
        originalPos = transform.position;
    }

    void FixedUpdate()
    {
        if (isChasing)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            rb.linearVelocity = direction * speed;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.transform;
            isChasing = true;
            CancelInvoke(nameof(Respawn));
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isChasing = false;
            rb.linearVelocity = Vector2.zero;
            Invoke(nameof(Respawn), respawnTime);
        }
    }



    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        Debug.Log(gameObject.name + " Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
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
        transform.position = originalPos;
        currentHealth = maxHealth;
        gameObject.SetActive(true);
    }
}
