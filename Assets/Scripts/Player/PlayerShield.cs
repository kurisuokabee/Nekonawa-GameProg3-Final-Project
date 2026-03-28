using System.Collections;
using UnityEngine;

public class PlayerShield : MonoBehaviour
{
    [SerializeField] GameObject shieldObject;
    [SerializeField] float shieldDuration = 3f;

    Coroutine shieldCoroutine;
    
    // activate shield
    public void ActivateShield()
    {
        // If already active, restart duration
        if (shieldCoroutine != null)
        {
            StopCoroutine(shieldCoroutine);
        }

        shieldCoroutine = StartCoroutine(ShieldRoutine());
    }

    IEnumerator ShieldRoutine()
    {
        shieldObject.SetActive(true);

        yield return new WaitForSeconds(shieldDuration);

        shieldObject.SetActive(false);
        shieldCoroutine = null;
    }
}