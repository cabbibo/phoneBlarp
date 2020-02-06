using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IMMATERIA;

public class BindPosition : Binder
{
    public Transform target;
    public string name;

    public override void Bind(){
      toBind.BindVector3(name, () => target.position );
    }
}
