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

    public float wallWidth;
    public GameObject[] walls;

    public void MakeWalls(){

      walls = new GameObject[4];


      hits = new Vector4[10];
      GameObject go;
        go = Instantiate( wallPrefab );
        go.transform.position = new Vector3( screen.width /2 , 0 , 0 );
        go.transform.localScale = new Vector3( wallWidth  , 10, screen.height );
        walls[0] = go;

        go = Instantiate( wallPrefab );
        go.transform.position = new Vector3( -screen.width /2, 0 , 0 );
        go.transform.localScale = new Vector3( wallWidth  , 10 ,screen.height );
        go.transform.rotation = Quaternion.AngleAxis( 180 , Vector3.up);
        walls[1] = go;

        go = Instantiate( wallPrefab );
        go.transform.position = new Vector3( 0 ,0 , screen.height/2 );
        go.transform.localScale = new Vector3( screen.width , 10 ,wallWidth );
        walls[2] = go;

        go = Instantiate( wallPrefab );
        go.transform.position = new Vector3( 0 , 0, -screen.height/2 );
        go.transform.localScale = new Vector3( screen.width , 10,wallWidth  );
        go.transform.rotation = Quaternion.AngleAxis( 180 , Vector3.up);
        walls[3] = go;


        wallMat = go.GetComponent<Renderer>().sharedMaterial;
        platform.transform.localScale = new Vector3( screen.width , 10, screen.height  ) * .1f; 

    }

    public void DestroyWalls(){
      if( walls.Length == 4 ){
      for( int i = 0; i <4; i++ ){
        DestroyImmediate( walls[i]);
      }
    }
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
