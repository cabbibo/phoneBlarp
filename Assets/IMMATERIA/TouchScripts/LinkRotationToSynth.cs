using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkRotationToSynth : MonoBehaviour
{

    public RotateWhileTouched rotation;
    public SampleSynth synth;
    public TouchToRay touch;



    // Update is called once per frame
    void FixedUpdate()
    {
        
        //synth.pitch = 2 + rotation.rotationSpeed;
        synth.speed = Mathf.Abs(1/(.001f+rotation.rotationSpeed* 10));
        synth.pitch = Mathf.Lerp( synth.pitch , Mathf.Abs( rotation.rotationSpeed ) , .1f);//Mathf.Sin( 10 * rotation.rotation / 360) + 1;
    }
}
