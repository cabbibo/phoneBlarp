using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IMMATERIA;

public class BindRigidBodyVelocity : Binder
{

  public Rigidbody body;

  public override void Bind(){
    toBind.BindVector3("_Velocity",() => body.velocity);
  }
}
