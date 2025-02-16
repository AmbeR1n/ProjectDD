using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{

    public static MusicManager Instance;
    
    [SerializeField]
    private float fadeTime = 0.5f; 
    [SerializeField]
    private MusicLibrary musicLibrary;
    [SerializeField]
    private AudioSource musicSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    public void PlayMusic(string trackName)
    {
        float fadeDuration = fadeTime;
        StartCoroutine(MusicFade(musicLibrary.GetClipFromName(trackName), fadeDuration));
    }
    IEnumerator MusicFade(AudioClip nextClip, float fadeDuration)
    {
        float percent = 0;
        while (percent < 1)
        {
            percent += Time.deltaTime * 1 / fadeDuration;
            musicSource.volume = Mathf.Lerp(1f, 0f, percent);
            yield return null;
        }

        musicSource.clip = nextClip;
        musicSource.Play();

        percent = 0;
        while (percent < 1)
        {
            percent += Time.deltaTime * 1 / fadeDuration;
            musicSource.volume = Mathf.Lerp(0f, 1f, percent);
            yield return null;
        }
    }
}
