using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IMMATERIA;

public class FollowTrail : Simulation
{

  public Transform leader;

  public override void Bind(){

    life.BindVector3( "_Leader" , ()=> leader.position );

  }

}
