using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicStartToLoop : MonoBehaviour
{
    public AudioClip startClip;
    public AudioClip loopClip;

    void OnEnable()
    {
        GetComponent<AudioSource> ().loop = true;
        StartCoroutine(playBGM());
    }

    public void SwitchBGM()
    {
        StopCoroutine(playBGM());
        StartCoroutine(playBGM());
    }
 
    IEnumerator playBGM()
    {
        GetComponent<AudioSource>().clip = startClip;
        GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(GetComponent<AudioSource>().clip.length);
        GetComponent<AudioSource>().clip = loopClip;
        GetComponent<AudioSource>().Play();
    }
}
