using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IMMATERIA {
public class ParticleTransferVerts: Form {


  public Particles particles;
  public int countMultiplier = 1;



    public override void _Create(){

      //print("Creating here");
    
    if( particles == null ){particles = GetComponent<Particles>(); }
    SetStructSize();
    SetCount();
    SetBufferType();
    DoCreate();
    Create();
  }
  public override void SetStructSize(){ structSize = 16; }

  public override void SetCount(){
    
    // 0-1
    // |/|
    // 2-3
    count = particles.count * 4 * countMultiplier;
  }

  public override void OnBirthed(){
//    print(_buffer);
  }

  

}



}