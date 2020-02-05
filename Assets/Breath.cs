using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breath : MonoBehaviour
{


  public TouchBlarp game;
  public float breathVal;
  public bool breathing;


public float coolDownTime;
public float coolDownSpeed;
public bool cooling;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {



      if( !breathing ){
        breathVal -= .01f;
      }else{
        breathVal += .03f;
      }

      if( breathVal < 0 && cooling == false){
        game.touchDown = false;
        TriggerCoolDown();
      }

      if( Time.time - coolDownTime > coolDownSpeed && cooling ){
        cooling = false;
        breathing = true;
        game.walls.wallMat.SetInt("_Cooling" , 0 );
      }

      breathVal = Mathf.Clamp( breathVal , 0 , 1 );

      transform.localScale = new Vector3(breathVal * game.screen.width, .3f  , .5f);
      transform.position = new Vector3( 0, 3,-game.screen.height * .5f + .25f);
    }

    public void TriggerCoolDown(){
      coolDownTime = Time.time;
      cooling = true;

      game.walls.wallMat.SetInt("_Cooling" , 1 );

    }
}
