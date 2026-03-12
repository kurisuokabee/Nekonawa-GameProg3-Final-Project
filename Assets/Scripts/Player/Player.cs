using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] int maxHealth = 100;
    public int currentHealth;
    public bool isDead = false;
    [SerializeField]GameObject playerSprite;
    [SerializeField]AudioClip audioClip;
    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {   
            SoundFXManager.Instance.PlaySound(audioClip, transform, 1f);
            TakeDamage(10);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log(gameObject.name + " died");
        playerSprite.SetActive(false);
        isDead = true;
        GameController.Instance.ChangeState(GameController.Instance.GameOverState);
    }

    public void Respawn()
    {
        currentHealth = maxHealth;
        isDead = false;
        playerSprite.SetActive(true);
    }
}
