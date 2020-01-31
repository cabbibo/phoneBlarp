using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuLayout : MonoBehaviour
{

  public ScreenInfo screen;
  public float sidesOffset;
  public float sidesVertical;
  public float titleVertical;

  public float forwardOffset;

  public GameObject currentScore;
  public GameObject highScore;

  public GameObject social;
  public GameObject title;


  // Start is called before the first frame update
  void Update()
  {

    currentScore.transform.position = new Vector3( sidesOffset * screen.width  , forwardOffset , sidesVertical * screen.height);
    highScore.transform.position    = new Vector3( -sidesOffset * screen.width  , forwardOffset , sidesVertical * screen.height);
      
   social.transform.position = new Vector3( 0 , forwardOffset , sidesVertical* screen.height );
   title.transform.position = new Vector3( 0 , forwardOffset , titleVertical * screen.height);
 }

}
