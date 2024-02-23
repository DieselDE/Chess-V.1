using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource[] soundEffects;

    public static SoundManager Instance;

    void Awake(){

        Instance = this;
    }

    public void PlaySound(SoundEffect soundEffect){

        soundEffects[(int)soundEffect].Play();
    }
}

public enum SoundEffect{
    gameStartSoundEffect = 0,
    gameEndSoundEffect = 1,
    moveSoundEffect = 2,
    moveCheckSoundEffect = 3,
    captureSoundEffect = 4,
    castleSoundEffect = 5,
    promoteSoundEffect = 6,
    illegalSoundEffect = 7,
    timeSoundEffect = 8
}