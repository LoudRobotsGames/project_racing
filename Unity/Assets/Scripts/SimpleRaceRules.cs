using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "SimpleRaceRules", menuName = "Cars/SimpleRaceRules", order = 2)]
public class SimpleRaceRules : GameRules
{
    private AudioSource playbackSource;

    public AudioClip three;
    public AudioClip two;
    public AudioClip one;
    public AudioClip go;

    public override void Begin()
    {
        playbackSource = GameController.Instance.SFXPlaybackSource;
        GameController.Instance.StartCoroutine(Countdown());
    }

    private IEnumerator Countdown()
    {
        playbackSource.clip = three;
        playbackSource.Play();
        WaitForSeconds wait = new WaitForSeconds(1f);
        yield return wait;

        playbackSource.clip = two;
        playbackSource.Play();
        yield return wait;

        playbackSource.clip = one;
        playbackSource.Play();
        yield return wait;

        playbackSource.clip = go;
        playbackSource.Play();
        yield return wait;
    }

    public override void Update()
    {
        throw new NotImplementedException();
    }
}
