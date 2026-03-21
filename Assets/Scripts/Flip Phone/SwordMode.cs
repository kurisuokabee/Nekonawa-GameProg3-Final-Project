using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SwordMode : MonoBehaviour, IPhone
{
    [SerializeField] float swingAngle = 120f;
    [SerializeField] float swingDuration = 0.2f;
    [SerializeField] float returnDuration = 0.15f;

    private bool isSwinging = false;
    HashSet<GameObject> hitEnemies = new HashSet<GameObject>();

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
        Debug.Log("Sword Mode Deactivated");
    }

    IEnumerator Swing()
    {
        isSwinging = true;
        hitEnemies.Clear();

        float startAngle = -swingAngle / 2f;
        float endAngle = swingAngle / 2f;

        float time = 0f;

        // Swing the sword
        while (time < swingDuration)
        {
            float t = time / swingDuration;
            t = Mathf.Sin(t * Mathf.PI); 

            float angle = Mathf.Lerp(startAngle, endAngle, t);
            transform.localRotation = Quaternion.Euler(0, 0, angle);

            time += Time.unscaledDeltaTime;
            yield return null;
        }

        // return to orginal position
        time = 0f;

        while (time < returnDuration)
        {
            float t = time / returnDuration;

            
            t = 1 - Mathf.Pow(1 - t, 2);

            float angle = Mathf.Lerp(startAngle, 0f, t);
            transform.localRotation = Quaternion.Euler(0, 0, angle);

            time += Time.unscaledDeltaTime;
            yield return null;
        }

        transform.localRotation = Quaternion.identity;

        isSwinging = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isSwinging) return;

        if (collision.CompareTag("Enemy") && !hitEnemies.Contains(collision.gameObject))
        {
            hitEnemies.Add(collision.gameObject);

            collision.GetComponent<Enemy>().TakeDamage(10);
        }
    }
}