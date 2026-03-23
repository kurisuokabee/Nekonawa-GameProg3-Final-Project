using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadingScreen : MonoBehaviour
{
    public static LoadingScreen Instance;
    [SerializeField]private Image loadingImage;
    [SerializeField]private GameObject loadingPanel;

     void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // make sure it's invisible at start
        Color c = loadingImage.color;
        c.a = 0f;
        loadingImage.color = c;
    }

    public void ShowLoading(float duration = 0.5f)
    {
        StartCoroutine(FadeRoutine(duration));
    }

    private IEnumerator FadeRoutine(float duration)
    {
        // Fade IN 
        float t = 0f;
        loadingPanel.SetActive(true);
        

        while (t < duration)
        {   
            t += Time.unscaledDeltaTime;
            float a = Mathf.Lerp(0f, 1f, t / duration);

            Color c = loadingImage.color;
            c.a = a;
            loadingImage.color = c;

            yield return null;
        }

        yield return new WaitForSecondsRealtime(1f);

        // Fade OUT )
        t = 0f;
        while (t < duration)
        {   
            t += Time.unscaledDeltaTime;
            float a = Mathf.Lerp(1f, 0f, t / duration);

            Color c = loadingImage.color;
            c.a = a;
            loadingImage.color = c;

            yield return null;
        }

      
        loadingPanel.SetActive(false);
    }
}