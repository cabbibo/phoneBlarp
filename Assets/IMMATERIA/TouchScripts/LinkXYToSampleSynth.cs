using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IMMATERIA;

public class LinkXYToSampleSynth : MonoBehaviour
{
   public SampleSynth synth;
   public TouchY touch;
   public TouchToRay t;

   public void Update(){
    synth.location = (touch.y +1) * 10 ;
    synth.volume = t.power* touch.down;
   }
}
