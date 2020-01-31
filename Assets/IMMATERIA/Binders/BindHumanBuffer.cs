using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IMMATERIA {
public class BindHumanBuffer : Binder
{

  public float _HumanForce;
  public float _HumanRadius;

  public override void Bind(){
    //toBind.BindForm("_HumanBuffer",data.humans);
    toBind.BindFloat( "_HumanRadius" ,() => _HumanRadius);
    toBind.BindFloat( "_HumanForce"  ,() => _HumanForce );
  }
   
}
}