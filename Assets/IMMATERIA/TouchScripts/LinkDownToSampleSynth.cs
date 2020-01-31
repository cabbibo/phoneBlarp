using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkDownToSampleSynth : MonoBehaviour
{
  public SampleSynth synth;
  public TouchToRay touch;

  public float vol;
  public float size;
  public Vector2 oP;

  void FixedUpdate(){


    if((touch.p - oP).magnitude > size ){


     // synth.pitch = (touchY.y + 1) /1.5f;
     // synth.location = (touchY.y + 1) * 10;
      synth.volume  = Mathf.Lerp( synth.volume , touch.Down * vol , .2f );
      synth.PlayGrain();

      oP = touch.p;

    }

  }

}
