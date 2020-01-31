using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkTouchToSampleSynth : MonoBehaviour
{
  public SampleSynth synth;
  public TouchY touchY;
  public TouchToRay touch;

  public float oY;

  void FixedUpdate(){


    if( Mathf.Abs( touchY.y - oY ) > .05 ){


     // synth.pitch = (touchY.y + 1) /1.5f;
      synth.location = (touchY.y + 1) * 10;
      synth.volume  = Mathf.Lerp( synth.volume , touch.Down , .2f );
      synth.PlayGrain();

      oY = touchY.y;
    }

  }

}
