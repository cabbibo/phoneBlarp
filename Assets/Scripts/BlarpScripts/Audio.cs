using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IMMATERIA;

public class Audio : MonoBehaviour
{
    public TouchBlarp game;

    public AudioPlayer audio;
    public AudioClip[] dieSounds;
    public AudioClip[] startSounds;
    public AudioClip[] colorSounds;
    public AudioClip[] tailSounds;
    public AudioClip[] targetSounds;
    public AudioClip[] wallSounds;
    public AudioSource closenessLoop;
    public AudioSource velocityLoop;



    public int[] steps;

    public void PlayClip( AudioClip[] clips ){
      audio.Play(clips[ Random.Range( 0 , clips.Length )] ,steps[ Random.Range(0,steps.Length )],1);
    }

    public void PlayClip( AudioClip[] clips, float volume){
      audio.Play(clips[ Random.Range( 0 , clips.Length )] ,steps[ Random.Range(0,steps.Length )],volume);
    }

    public void PlayClip( AudioClip[] clips, float volume , float pitch){
      audio.Play(clips[ Random.Range( 0 , clips.Length )] ,pitch,volume);
    }

    public void PlayRestart(){
      PlayClip(dieSounds);
    }

    public void PlayStart(){

    }

    public void PlayHighScore(){

    }

    public void PlayTouchSound( Vector2 mousePos ){
           //touchLR.SetPosition( 0 , blarp.transform.position + dif * .2f -Camera.main.transform.forward * upConnectionDist * 2);
           //touchLR.SetPosition( 1 , blarp.transform.position + dif * .5f -Camera.main.transform.forward * upConnectionDist * 2);
           //touchLR.SetWidth(.6f, 0);

           //touchAudioSource.pitch  =  Mathf.Clamp( 4/(touch.transform.position - blarp.transform.position).magnitude,0 , 10);
           //touchAudioSource.volume = Mathf.Lerp( touchAudioSource.volume , 1 , .5f );
    }

    public void PlayTargetHit(){
      print("playing herer");
      PlayClip( targetSounds );
    }

    public void PlayTailTargetHit(){

    }

    public void PlayColorTargetHit(){
      print("hiii");
PlayClip(colorSounds);
    }

    public void DoWallHit(float magnitude){
      print( magnitude + " !!!!!!!!!!!!!!!!!!");
      PlayClip(wallSounds,1,magnitude / 4);
    }

    public void DoCloseness(){

      float target = 4 / (game.blarp.transform.position - game.shark.transform.position).magnitude;
      closenessLoop.volume = Mathf.Lerp( closenessLoop.volume , target , .1f );
      closenessLoop.pitch = Mathf.Lerp( closenessLoop.pitch , target , .1f );

    }

    public void Awake(){

    }


    void Update(){
      DoCloseness();
    }
}
