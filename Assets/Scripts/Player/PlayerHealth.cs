using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] int maxHealth = 100;
    public int currentHealth;
    public bool isDead = false;
    [SerializeField]GameObject playerSprite;
    [SerializeField]AudioClip audioClip;
    [SerializeField] Slider healthBar;
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.value = maxHealth;
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
        healthBar.value = currentHealth;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int heal)
    {   
        currentHealth += heal;
        healthBar.value = currentHealth;

        if(currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
            healthBar.value = currentHealth;
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
        healthBar.value = maxHealth;
        isDead = false;
        playerSprite.SetActive(true);
    }
}
