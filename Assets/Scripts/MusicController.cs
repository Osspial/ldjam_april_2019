﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    private MusicTrack currentTrack;
    public AudioSource audioSource;
    public float volume = 1;

    private Coroutine coroutine;
    public void StartTrack(MusicTrack track)
    {
        if (track != currentTrack)
        {
            coroutine = StartCoroutine(TransitionTrack(currentTrack, track, coroutine));
            currentTrack = track;
        }
    }
    public void KillMusic()
    {
        audioSource.Stop();
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
    }

    IEnumerator TransitionTrack(MusicTrack from, MusicTrack to, Coroutine stopCoroutine)
    {
        if (from != null)
        {
            while (audioSource.volume > 0)
            {
                audioSource.volume -= (1f / from.fadeOut) * Time.deltaTime;
                yield return null;
            }
            audioSource.Stop();
        }
        if (stopCoroutine != null)
        {
            StopCoroutine(stopCoroutine);
        }

        audioSource.volume = volume;
        if (to != null)
        {
            audioSource.PlayOneShot(to.intro);
            yield return new WaitForSecondsRealtime(to.introTime);
            while (true)
            {
                audioSource.PlayOneShot(to.body);
                yield return new WaitForSecondsRealtime(to.bodyTime);
            }
        }
    }
}
