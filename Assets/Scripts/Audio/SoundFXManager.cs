using System.Collections;
using System.Xml.Serialization;
using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager Instance;

    [SerializeField] AudioSource[] pool;

    int index = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        foreach(AudioSource a in pool)
            a.gameObject.SetActive(false);
    }

    public void PlaySound(AudioClip clip, Transform pos, float volume)
    {
        AudioSource source = GetObject();

        source.transform.position = pos.position;
        source.clip = clip;
        source.volume = volume;

        source.Play();

        StartCoroutine(DisableAfterPlayingSound(source, clip.length)); 
    }

    IEnumerator DisableAfterPlayingSound(AudioSource source, float time)
    {
        yield return new WaitForSecondsRealtime(time);
        source.clip = null;
        source.gameObject.SetActive(false);
    }

    AudioSource GetObject()
    {
        AudioSource obj = pool[index];

        index = (index + 1) % pool.Length;

        obj.gameObject.SetActive(true);

        return obj;
    }
}
