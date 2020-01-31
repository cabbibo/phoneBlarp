using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



[System.Serializable]
public class Vector2Event : UnityEvent<Vector2>{}

[System.Serializable]
public class Vector3Event : UnityEvent<Vector3>{}


[System.Serializable]
public class Vector2FloatEvent : UnityEvent<Vector2,float>{}

[System.Serializable]
public class FloatEvent : UnityEvent<float>{}


[System.Serializable]
public class RayEvent : UnityEvent<Ray>{}

public class TouchToRay : MonoBehaviour {

  public Vector2Event OnSwipe;
  public FloatEvent OnSwipeHorizontal;
  public UnityEvent OnSwipeLeft;
  public UnityEvent OnSwipeRight;
  public FloatEvent OnSwipeVertical;
  public UnityEvent OnSwipeUp;
  public UnityEvent OnSwipeDown;
  public UnityEvent OnTap;
  public UnityEvent OnDown;
  public UnityEvent OnUp;
  public RayEvent WhileDown;
  public Vector2Event WhileDown2;

  public UnityEvent OnDebugTouch;
  

  public Vector3 RayOrigin;
  public Vector3 RayDirection;
  
  public float Down;
  public float oDown;
  
  public float Down2;
  public float oDown2;

  public float JustDown;
  public float JustUp;
  public Vector2 startPos;
  public Vector2 endPos;

  public Ray ray;
  public Ray oRay;

  public float startTime;
  public float endTime;
  // Use this for initialization

  public Vector2 p; 
  public Vector2 oP; 
  public Vector2 vel;

  public Vector2 p2;
  public Vector2 oP2;
  public Vector2 vel2;

  public int touchID = 0;

  public string currentHitName;
  public Vector3 currentHitLocation;
  public string downHitName;
  public float power;

  void Start(){}
  
   // Update is called once per frame
  void FixedUpdate () {

    oP = p;
    oDown = Down;

    oP2 = p2;
    oDown2 = Down2;

    #if UNITY_EDITOR || UNITY_STANDALONE  
      if (Input.GetMouseButton (0)) {
        Down = 1;
        p  =  Input.mousePosition;///Input.GetTouch(0).position;


      }else{
        Down = 0;
        oP = p;
      }

      if( Input.GetMouseButtonDown(0) &&   Input.GetKey("space") ){
        OnDebugTouch.Invoke();
      }

      if( Input.GetMouseButton(1)){
       
         p2 =   Input.mousePosition;
         Down2 = 1;
      }else{
        Down2 = 0;
        oP2 = oP;
      }
    #else
      if (Input.touchCount == 1 ){
        Down = 1;
        p  =  Input.GetTouch(0).position;

        if( Input.touchCount > 2 ){
          if( Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(1).phase == TouchPhase.Began || Input.GetTouch(2).phase == TouchPhase.Began ){
            OnDebugTouch.Invoke();
          }
        }
      }else{
        Down = 0;
        oP = p;
      }

      if (Input.touchCount == 2 ){
        Down2 = 1;
        p2  =  Input.GetTouch(0).position;

        if( Input.touchCount > 2 ){
          if( Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(1).phase == TouchPhase.Began || Input.GetTouch(2).phase == TouchPhase.Began ){
            OnDebugTouch.Invoke();
          }
        }
      }else{
        Down2 = 0;
        oP2 = p2;
      }
    #endif

    

    oRay.origin = ray.origin;
    oRay.direction = ray.direction;

    RayOrigin = Camera.main.ScreenToWorldPoint( new Vector3( p.x , p.y , Camera.main.nearClipPlane ) );
    RayDirection = -(Camera.main.transform.position - RayOrigin).normalized;


    ray.origin = RayOrigin;
    ray.direction = RayDirection;//.normalized;




      if( Down == 1 && oDown == 0 ){
          JustDown = 1;
          touchID ++;
          startTime = Time.time;
          startPos = p;
          whileDown();
          onDown();
      }


      if( Down == 1 && oDown == 1 ){
        JustDown = 0;
        whileDown();
      }

      if( Down2 == 1 && oDown2 == 1 ){
        whileDown2();
      }



      if( Down == 0 && oDown == 1 ){
        JustUp = 1;
        endTime = Time.time;
        endPos = p;
        onUp();
      }      

      if( Down == 0 && oDown == 0 ){
        JustDown = 0;
      }

      if( JustDown == 1 ){ oP = p; }
      vel = p - oP;


  
      if( Down == 1 ){
        power += .1f;
      }else{
        power -= .02f;
      }

      power = Mathf.Clamp( power , 0 , 1);



  }

  void whileDown(){

    RaycastHit hit;
     // Does the ray intersect any objects excluding the player layer
    if (Physics.Raycast(RayOrigin,RayDirection, out hit, Mathf.Infinity)){
       currentHitLocation = hit.point;
       currentHitName = hit.collider.name;
    }else{
      currentHitLocation =Vector3.zero;

      currentHitName = null;
    }

        
   
    WhileDown.Invoke( ray );
  }

  void whileDown2(){
    WhileDown2.Invoke( p2 - oP2 );
  }


  void onDown(){
    OnDown.Invoke();

    RaycastHit hit;
     // Does the ray intersect any objects excluding the player layer
    if (Physics.Raycast(RayOrigin,RayDirection, out hit, Mathf.Infinity)){
       downHitName = hit.collider.name;
    }else{
      downHitName = null;
    }


  }

  void onUp(){

    OnUp.Invoke();
    float difT = endTime - startTime;
    Vector2 difP = endPos - startPos;


    float ratio = .01f * difP.magnitude / difT;

    if( ratio > 3 ){
      
      OnSwipe.Invoke( difP );  
     if( Mathf.Abs(difP.x) > Mathf.Abs(difP.y) ){
        OnSwipeHorizontal.Invoke(difP.x);
        if( difP.x < 0 ){
          OnSwipeLeft.Invoke();
        }else{
          OnSwipeRight.Invoke();
        }
     }else{
      OnSwipeVertical.Invoke(difP.y);
      if( difP.x < 0 ){
        OnSwipeUp.Invoke();
      }else{
        OnSwipeDown.Invoke();
      }
     } 
    }else{
      OnTap.Invoke();
    }

    currentHitName = null;

   //print( difT );
   //print( difP );
   //print( ratio );
  }


}