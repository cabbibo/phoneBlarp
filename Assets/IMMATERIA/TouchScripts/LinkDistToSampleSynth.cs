using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkDistToSampleSynth : MonoBehaviour
{

    public DragFromLocation location;
    public SampleSynth synth;

    void Update(){

      float dist = (location.targetLocation.position - transform.position).magnitude;

    synth.pitch = dist;
    synth.speed = .1f / dist;

    }
}
