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
    [SerializeField] float speed = 5f;

    [SerializeField] Slider healthBar;

    bool isChasing = false;
    

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {   
        currentHealth = maxHealth;
        originalPos = transform.position;
        healthBar.value = maxHealth;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj)
            player = playerObj.transform;
    }

    void FixedUpdate()
    {
        if (isChasing && player != null)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            rb.linearVelocity = direction * speed;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
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
        healthBar.value = maxHealth;
        isChasing = false;
        gameObject.SetActive(true);
    }
}
