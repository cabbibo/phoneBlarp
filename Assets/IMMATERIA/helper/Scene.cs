using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace IMMATERIA {
public class Scene : Cycle {
public  Transform view;
public void OnEnable(){

    Reset();
    _Destroy(); 
    _Create(); 
    _OnGestate();
    _OnGestated();
    _OnBirth(); 
    _OnBirthed();

    _OnLive(); 

}


public void OnDisable(){
  _Destroy();
}





}
}