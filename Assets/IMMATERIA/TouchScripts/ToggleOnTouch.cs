using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleOnTouch : MonoBehaviour
{

  public TouchToRay touch;
  public bool on;

  public void Touch(){

    print("hesls");
    if( touch.currentHitName == touch.downHitName && touch.currentHitName == gameObject.name ){
    on = !on;

    if( on ){ OnOn(); }else{OnOff();}

    }
  }




  public void OnOn(){}

  public void OnOff(){}
}
