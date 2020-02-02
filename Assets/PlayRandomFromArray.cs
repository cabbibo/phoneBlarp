using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IMMATERIA;

public class PlayRandomFromArray : MonoBehaviour
{

    public AudioClip[] clips;
    public AudioPlayer audio;
    public int[] steps;
    public void Play(){
      audio.Play( clips[ Random.Range( 0 , clips.Length )], steps[ Random.Range( 0 , steps.Length )] , 1 );
    }
    
}
