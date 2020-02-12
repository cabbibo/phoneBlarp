using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IMMATERIA;

public class Audio : MonoBehaviour
{

    public AudioPlayer audio;
    public AudioClip[] dieSounds;
    public AudioClip[] startSounds;
    public AudioClip[] colorSounds;
    public AudioClip[] tailSounds;
    public AudioClip[] targetSounds;
    public AudioClip[] wallSounds;
    public AudioClip closenessLoop;
    public AudioClip velocityLoop;



    public int[] steps;

    public void PlayClip( AudioClip[] clips ){
      audio.Play(clips[ Random.Range( 0 , clips.Length )] ,1 ,steps[ Random.Range(0,steps.Length )]);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
