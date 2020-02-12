using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IMMATERIA;

public class BindIsSpawned : Binder{

  public TargetInfo target;
    public override void Bind(){
      toBind.BindInt( "_IsSpawned" , () => target.spawned );
    }
}
