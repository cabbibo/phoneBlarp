using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchY : MonoBehaviour
{
    public TouchToRay touch;


  public float y;
  public float x;
  public float z;
  public float down;
  
  public void Touch( Ray r ){
    if(touch.currentHitName == gameObject.name && touch.downHitName == gameObject.name){

      Vector3 local = transform.InverseTransformPoint(touch.currentHitLocation);
      y = local.z;
      x = local.y;
      z = local.x;
      down = 1;
    }else{
      down = 0;
    }
  }
}
