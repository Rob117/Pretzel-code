using UnityEngine;
using System.Collections;
using System;

public class MusicManager : Singleton<MusicManager> {

    public bool random = false;
    public int clipNumber;

    public AudioClip[] music;
    public AudioSource source;

    public void Start() {
        if (random) {
            var r = new System.Random();
            clipNumber = r.Next(0, music.Length);
        }
        if (!music[clipNumber])
            return;

        source.clip = music[clipNumber];
        source.Play();
    }
}
