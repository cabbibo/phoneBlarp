using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IMMATERIA;

public class BindTwoPositions : Binder
{

public Transform position1;
public Transform position2;

public override void Bind(){
  toBind.BindVector3( "_Position1" , () => position1.position );
  toBind.BindVector3( "_Position2" , () => position2.position );
}


}
