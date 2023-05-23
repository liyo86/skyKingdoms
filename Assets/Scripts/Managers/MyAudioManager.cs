using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyAudioManager : MonoBehaviour
{
    public static MyAudioManager Instance;
    public AudioSource sourceSFX;
    public AudioSource sourceMusic;
   

    // Music
    public AudioClip cinematic;
    public AudioClip dayAmbient;
    public AudioClip flight;
    public AudioClip boss;
    public AudioClip dungeon;
    
    // SFX
    public AudioClip ringSFX;
    public AudioClip bossAttack1SFX;
    public AudioClip gameOverSFX;
    public AudioClip levelCompleteSFX;
    
    // Voice
    public AudioClip fireVoice;
    public AudioClip defenseVoice;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }


    public void PlaySfx(string sfxName)
    {
        switch (sfxName)
        {
            case nameof(ringSFX):
                sourceSFX.PlayOneShot(ringSFX);
                break;
            case nameof(fireVoice):
                sourceSFX.PlayOneShot(fireVoice);
                break;
            case nameof(gameOverSFX):
                sourceSFX.PlayOneShot(gameOverSFX);
                break;
            case nameof(levelCompleteSFX):
                sourceSFX.PlayOneShot(levelCompleteSFX);
                break;
            case nameof(bossAttack1SFX):
                sourceSFX.PlayOneShot(bossAttack1SFX);
                break;
            case nameof(defenseVoice):
                sourceSFX.PlayOneShot(defenseVoice);
                break;
        }
    }
    
    public void PlayMusic(string musicName)
    {
        switch (musicName)
        {
            case nameof(cinematic):
                sourceMusic.clip = cinematic;
                sourceMusic.Play();
                break;
            case nameof(dayAmbient):
                sourceMusic.clip = dayAmbient;
                sourceMusic.Play();
                break;
            case nameof(flight):
                sourceMusic.clip = flight;
                sourceMusic.Play();
                break;
            case nameof(boss):
                sourceMusic.clip = boss;
                sourceMusic.Play();
                break;
            case nameof(dungeon):
                sourceMusic.clip = dungeon;
                sourceMusic.Play();
                break;
            default:
                sourceMusic.clip = cinematic;
                sourceMusic.Play();
                break;
        }
    }

    public void StopAny()
    {
        sourceSFX.Stop();
        sourceMusic.Stop();
    }
}
