using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicFades : MonoBehaviour
{
    private AudioSource audioSource;
    public float fadeTime = 3;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public IEnumerator FadeOut()
    {
        StopCoroutine(FadeIn());
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeTime;
            yield return null;
        }
    }

    public IEnumerator FadeIn()
    {
        StopCoroutine(FadeOut());
        float startVolume = audioSource.volume + 0.5f;

        while (audioSource.volume < 1)
        {
            audioSource.volume += startVolume * Time.deltaTime / fadeTime;
            yield return null;
        }
    }
}
