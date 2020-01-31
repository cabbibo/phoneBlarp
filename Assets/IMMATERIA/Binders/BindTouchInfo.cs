using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IMMATERIA;

public class BindTouchInfo : Binder
{

    public TouchToRay touch;

    public float touchPower;
    public Vector3 lastTouchLocation;
    
    public override void Bind(){
      toBind.BindVector3("_RO" , () => touch.RayOrigin );
      toBind.BindVector3("_RD" , () => touch.RayDirection );
      toBind.BindVector3("_TouchLocation" , () => lastTouchLocation );
      toBind.BindFloat("_TouchPower" , () => touchPower );
    }

    public override void WhileLiving( float v ){

      if( touch.currentHitLocation != Vector3.zero ){
        lastTouchLocation = touch.currentHitLocation;
      }

      if( touch.Down == 1 ){
        touchPower += .02f;
      }else{
        touchPower -= .005f;
      }

      touchPower = Mathf.Clamp( touchPower , 0 , 1);

    }

}
