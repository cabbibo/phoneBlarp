using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWhileTouched : MonoBehaviour
{

  public TouchToRay touch;

  public float rotationSpeed;
  public float rotationVel;
  public float rotation;

  public Vector3 oHit;

    public void OnEnable(){
    touch.WhileDown.AddListener( Rotate );
   }

   public void OnDisable(){
    touch.WhileDown.RemoveListener( Rotate);
   }

  public void Rotate( Ray r ){
    if(touch.currentHitName == gameObject.name && touch.downHitName == gameObject.name ){

      print("hi");

      if( oHit != Vector3.zero  && touch.currentHitLocation != Vector3.zero && touch.JustDown == 0 ){
        print("doubleHi");
        Vector3 cross = Vector3.Cross( new Vector3( touch.vel.x , touch.vel.y , 0 ) , Vector3.forward );

        Vector3 dif = Vector3.Cross( (touch.currentHitLocation - transform.position), (oHit - transform.position)); 
        float v = Vector3.Dot(dif, transform.forward);


        rotationSpeed -= .5f * v;

        oHit = touch.currentHitLocation;
      }else{
      }
    }
  }

  public void FixedUpdate(){

      rotationSpeed *= .99f;

        oHit = touch.currentHitLocation;
      transform.Rotate(Vector3.forward * rotationSpeed);
      rotation = transform.eulerAngles.z;
  }


}
