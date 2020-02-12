using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IMMATERIA;

public class BindLerpVal : Binder
{

    public TailGrowerChanger tail;
    

    public override void Bind(){
      toBind.BindFloat("_ScaleLerpVal" , () => tail.lerpScale );
    }
}
