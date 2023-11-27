using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : SimpleSingleton<AudioManager>
{
    [SerializeField] private AudioSource audioSourceBGM1, audioSourceBGM2;
    [SerializeField] private SimpleObjectPool audioSourceSFXpool;
    [SerializeField] private AudioDatabase audioList;

    private Music[] musicList;
    private Sound[] soundList;

    [SerializeField] private float currentMusicVolume = 0.8f;
    [SerializeField] private float previousMusicVolume = 0f;
    private Music currentMusic;
    private float transitionTime = 1.25f;

    private void Start()
    {
        musicList = audioList.MusicList;
        soundList = audioList.SoundList;
    }

    public void PlayMusic(string name)
    {
        Music target = Array.Find(musicList, sound => sound.name == name);
        currentMusic = target;

        previousMusicVolume = currentMusicVolume;
        currentMusicVolume = target.volume;
        ChangeClip();
    }

    public void PlaySound(string name)
    {
        Sound target = Array.Find(soundList, sound => sound.name == name);
        GameObject audioObj = audioSourceSFXpool.GetPooledObject();
        AudioSource audio = audioObj.GetComponent<AudioSource>();

        audio.clip = target.clip;
        audio.volume = target.volume;
        audioObj.SetActive(true);
        audio.PlayOneShot(audio.clip);
    }

    private void ChangeClip()
    {
        AudioSource nowPlaying = audioSourceBGM1;
        AudioSource target = audioSourceBGM2;

        if (!nowPlaying.isPlaying)
        {
            nowPlaying = audioSourceBGM2;
            target = audioSourceBGM1;
        }

        target.clip = currentMusic.clip;
        StartCoroutine(Transition(nowPlaying, target));
    }

    private IEnumerator Transition(AudioSource now, AudioSource next)
    {
        float rate = 0;

        while(now.volume > 0)
        {
            now.volume = Mathf.Lerp(previousMusicVolume, 0, rate);
            rate += Time.deltaTime / transitionTime;
            yield return null;
        }

        now.Stop();
        if (!next.isPlaying)
        {
            next.Play();
        }
        rate = 0;

        while(next.volume < currentMusicVolume)
        {
            next.volume = Mathf.Lerp(0, currentMusicVolume, rate);
            rate += Time.deltaTime / transitionTime;
            yield return null;
        }
    }
}
