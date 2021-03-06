﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "IntroCountdownRules", menuName = "Cars/IntroCountdownRules", order = 2)]
public class IntroCountdownRules : GameRules
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
        List<VehicleBase> vehicles = GameController.Instance.Vehicles;
        for (int i = 0; i < vehicles.Count; ++i)
        {
            vehicles[i].Stop();
        }
    }

    private IEnumerator Countdown()
    {
        WaitForSeconds wait = new WaitForSeconds(1f);

        // Three
        GuiController.Instance.ShowNotification("3", 1f);
        playbackSource.clip = three;
        playbackSource.Play();
        yield return wait;

        // Two
        GuiController.Instance.ShowNotification("2", 1f);
        playbackSource.clip = two;
        playbackSource.Play();
        yield return wait;

        // One
        GuiController.Instance.ShowNotification("1", 1f);
        playbackSource.clip = one;
        playbackSource.Play();
        yield return wait;

        // GO
        GuiController.Instance.ShowNotification("GO!", 1f);
        playbackSource.clip = go;
        playbackSource.Play();

        List<VehicleBase> vehicles = GameController.Instance.Vehicles;
        for (int i = 0; i < vehicles.Count; ++i)
        {
            vehicles[i].Release();
        }

        yield return wait;

        if (OnFinish != null)
        {
            OnFinish();
        }
    }

    public override void Update()
    {
    }
}
