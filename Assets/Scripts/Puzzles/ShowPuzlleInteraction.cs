using UnityEngine;

public class ShowPuzlleInteraction : MonoBehaviour
{
    public GameObject uiPrefab; 
    private GameObject currentUIInstance;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && currentUIInstance == null)
        {
            currentUIInstance = Instantiate(uiPrefab, transform);
            Debug.Log("UI shown");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && currentUIInstance != null)
        {
            Destroy(currentUIInstance);
            currentUIInstance = null;
            Debug.Log("UI hidden");
        }
    }
}
