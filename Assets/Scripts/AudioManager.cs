using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager AudioInstance;

    public AudioSource bgmSource;

    public AudioClip ballKickSfx;
    public AudioClip buttonSfx;

    private void Awake()
    {
        if (AudioInstance == null)
        {
            AudioInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayBallKickSfx()
    {
        bgmSource.PlayOneShot(ballKickSfx);
    }

    public void PlayButtonSfx()
    {
        bgmSource.PlayOneShot(buttonSfx);
    }
}
