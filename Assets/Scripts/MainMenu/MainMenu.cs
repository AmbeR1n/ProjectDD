using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using System;
using System.Collections.Generic;
public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private Button[] buttons;
    [SerializeField]
    private Slider musicSlider;
    [SerializeField]
    private Slider soundSlider;
    [SerializeField]
    private AudioMixer audioMixer;

    void Start()
    {
        foreach (var button in buttons)
        {
            var trigger = button.GetComponent<EventTrigger>();
            AddEventTriggerListener(trigger, EventTriggerType.PointerEnter, OnPointerHoverListener);
        }

        LoadVolume();
        MusicManager.Instance.PlayMusic("MainMenu");
    }

    private void OnPointerHoverListener(BaseEventData data)
    {
        SoundManager.Instance.PlaySound2D("Hover");
    }

    public void Play()
    {
        SceneManager.LoadScene("Game");
        MusicManager.Instance.PlayMusic("Game");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void UpdateMusicVolume(float volume)
    {
        audioMixer.SetFloat("musicVolume", volume);
    }

    public void UpdateSoundVolume(float volume)
    {
        audioMixer.SetFloat("soundVolume", volume);
    }

    public void SaveVolume()
    {
        audioMixer.GetFloat("musicVolume", out float musicVolume);
        audioMixer.GetFloat("soundVolume", out float soundVolume);

        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.SetFloat("SoundVolume", soundVolume);
    }
    private void LoadVolume()
    {
        if (musicSlider != null)
            musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        if (soundSlider != null)
            soundSlider.value = PlayerPrefs.GetFloat("SoundVolume");
    }

    public static void AddEventTriggerListener(EventTrigger trigger, EventTriggerType eventType, System.Action<BaseEventData> callback)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = eventType;
        entry.callback = new EventTrigger.TriggerEvent();
        entry.callback.AddListener(new UnityEngine.Events.UnityAction<BaseEventData>(callback));
        trigger.triggers.Add(entry);
    }
}