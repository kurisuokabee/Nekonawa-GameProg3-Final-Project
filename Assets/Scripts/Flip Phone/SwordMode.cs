using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SwordMode : MonoBehaviour, IPhone
{
    private bool isSwinging = false;
    private HashSet<GameObject> hitEnemies = new HashSet<GameObject>();

    [SerializeField] GameObject swordSlashFX;
    [SerializeField] int damage = 10;

    public void Use()
    {
        if (!isSwinging)
            StartCoroutine(Swing());
    }

    public void Enter()
    {
        Debug.Log("Sword Mode Activated");
    }

    public void Exit()
    {
        isSwinging = false;
        if (swordSlashFX != null)
            swordSlashFX.SetActive(false);
        hitEnemies.Clear();
        Debug.Log("Sword Mode Deactivated");
    }

    IEnumerator Swing()
    {
        isSwinging = true;
        hitEnemies.Clear();

        if (swordSlashFX != null)
        {
            // Activate FX
            swordSlashFX.SetActive(true);

            // Checks if it hits enemy
            Collider2D swordCollider = GetComponent<Collider2D>();
            if (swordCollider != null)
            {
                ContactFilter2D filter = new ContactFilter2D();
                filter.SetLayerMask(LayerMask.GetMask("Enemy")); 
                filter.useTriggers = true;

                Collider2D[] results = new Collider2D[10];
                int count = swordCollider.Overlap(filter, results);

                for (int i = 0; i < count; i++)
                {
                    GameObject enemy = results[i].gameObject;
                    if (!hitEnemies.Contains(enemy))
                    {
                        hitEnemies.Add(enemy);
                        enemy.GetComponent<Enemy>().TakeDamage(damage);
                    }
                }
            }

            // Wait for FX animation to finish
            Animator animator = swordSlashFX.GetComponent<Animator>();
            if (animator != null)
            {
                yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
            }
            else
            {
                yield return new WaitForSeconds(0.5f); 
            }

            swordSlashFX.SetActive(false);
        }

        isSwinging = false;
    }

    // Detect enemies entering mid-swing
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isSwinging) return;

        if (collision.CompareTag("Enemy") && !hitEnemies.Contains(collision.gameObject))
        {
            hitEnemies.Add(collision.gameObject);
            collision.GetComponent<Enemy>().TakeDamage(damage);
        }
    }
}