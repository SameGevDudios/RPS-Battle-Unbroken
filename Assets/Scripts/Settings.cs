using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    bool opened;
    Animator anim;
    public Animator stickAnim;


    public Slider musicSlider;
    public Slider soundSlider;
    AudioSource musicAS;
    AudioSource soundAS;

    void Start()
    {
        anim = GetComponent<Animator>();
        musicAS = GameObject.FindGameObjectWithTag("music").GetComponent<AudioSource>();
        soundAS = GameObject.FindGameObjectWithTag("soundManager").GetComponent<AudioSource>();

        SetMusic();
        SetSound();
        UpdateSliders();
    }


    public void Open()
    {
        opened = !opened;
        if (!opened)
        {
            anim.SetBool("opened", false);
            stickAnim.Play("close");
        }
        else
        {
            anim.SetBool("opened", true);
            stickAnim.Play("open");
        }
    }

    public void UpdateMusic()
    {
        SaveMusic(musicSlider.value);
        SetMusic();
    }

    public void UpdateSound()
    {
        SaveSound(soundSlider.value);
        SetSound();
    }

    private void SaveMusic(float value)
    {
        PlayerPrefs.SetFloat("musicVolume", value);
    }

    private void SaveSound(float value)
    {
        PlayerPrefs.SetFloat("soundVolume", value);
    }

    private void SetMusic()
    {
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            musicAS.volume = PlayerPrefs.GetFloat("musicVolume");
        }
        else
        {
            musicAS.volume = 1f;
        }
    }

    private void SetSound()
    {
        if (PlayerPrefs.HasKey("soundVolume"))
        {
            soundAS.volume = PlayerPrefs.GetFloat("soundVolume");
        }
        else
        {
            soundAS.volume = 1f;
        }
    }

    private void UpdateSliders()
    {
        musicSlider.value = musicAS.volume;
        soundSlider.value = soundAS.volume;
    }
}
