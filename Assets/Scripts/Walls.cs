using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walls : MonoBehaviour
{


    public GameObject wallPrefab;
    public GameObject platform;

    public ScreenInfo screen;



    public Vector4[] hits;
public int currHit;

    public Material wallMat;


    public void MakeWalls(){

      hits = new Vector4[10];
      GameObject go;
        go = Instantiate( wallPrefab );
        go.transform.position = new Vector3( screen.width /2 , 0 , 0 );
        go.transform.localScale = new Vector3( .1f  , 10, screen.height );
      
        wallMat = go.GetComponent<Renderer>().sharedMaterial;

        go = Instantiate( wallPrefab );
        go.transform.position = new Vector3( -screen.width /2, 0 , 0 );
        go.transform.localScale = new Vector3( .1f  , 10 ,screen.height );

        go = Instantiate( wallPrefab );
        go.transform.position = new Vector3( 0 ,0 , screen.height/2 );
        go.transform.localScale = new Vector3( screen.width , 10 ,.1f );

        go = Instantiate( wallPrefab );
        go.transform.position = new Vector3( 0 , 0, -screen.height/2 );
        go.transform.localScale = new Vector3( screen.width , 10,.1f  );

        platform.transform.localScale =new Vector3( screen.width , 10, screen.height  ) * .1f; 

    }


public void SetWallCollision(Vector3 location){

  print( location );
  currHit += 1;
  currHit %= hits.Length;
  hits[0] = new Vector4( location.x , location.y , location.z , Time.time );

  wallMat.SetVectorArray( "_Hits" , hits );

  print("hi");


}
}
