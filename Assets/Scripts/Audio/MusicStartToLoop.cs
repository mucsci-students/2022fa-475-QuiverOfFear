using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicStartToLoop : MonoBehaviour
{
    public AudioClip startClip, loopClip;
    public AudioClip level1StartClip;
    public AudioClip level1LoopClip;
    public AudioClip level2StartClip;
    public AudioClip level2LoopClip;
    public AudioClip level3StartClip;
    public AudioClip level3LoopClip;

    void OnEnable()
    {
        GetComponent<AudioSource> ().loop = true;

        switch (PlayerPrefs.GetInt("level"))
        {
            case 1:
                startClip = level2StartClip;
                loopClip = level2LoopClip;
                break;
            case 2:
                startClip = level3StartClip;
                loopClip = level3LoopClip;
                break;
            default:
                startClip = level1StartClip;
                loopClip = level1LoopClip;
                break;
        }

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
