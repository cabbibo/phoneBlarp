using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchFlick : MonoBehaviour
{
   
   public TouchToRay touch;
   public Rigidbody rb;

   public void OnEnable(){
    rb = GetComponent<Rigidbody>();
    touch.WhileDown.AddListener( Touch );
   }

   public void OnDisable(){
    rb = GetComponent<Rigidbody>();
    touch.WhileDown.RemoveListener( Touch);
   }


   public void Touch( Ray r ){  
    if( touch.downHitName == gameObject.name ){
      Vector3 dir = new Vector3( touch.vel.x , touch.vel.y , 0 );

      dir = Camera.main.transform.TransformVector( dir );
      rb.AddForce( dir * .1f );
    }
   }
}
