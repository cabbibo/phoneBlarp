using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IMMATERIA;

[ExecuteInEditMode]
public class BindServerTimeToAnimation : MonoBehaviour
{

  public Data data;

  void Update(){
    Animation anim = GetComponent<Animation>();
    anim["MyClip"].time = data.SERVER_TIME;
    anim["MyClip"].enabled = true;
    anim.Sample();
    anim["MyClip"].enabled = false;
  }
}
