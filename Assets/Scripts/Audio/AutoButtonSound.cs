using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class AutoButtonSound : MonoBehaviour
{
    public static AutoButtonSound Instance;

    public AudioClip clickSound;

    void Awake()
    {
        // Singleton (only one allowed)
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Listen to scene changes
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        ApplyToAllButtons();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ApplyToAllButtons();
    }

   
    void ApplyToAllButtons()
    {
        Button[] buttons = FindObjectsByType<Button>(FindObjectsSortMode.None);

        foreach (Button btn in buttons)
        {
            btn.onClick.RemoveListener(PlayClick); // prevent duplicates
            btn.onClick.AddListener(PlayClick);
        }
    }

    

    void PlayClick()
    {
        SoundFXManager.Instance.PlaySound(clickSound, transform,  1);
    }
}