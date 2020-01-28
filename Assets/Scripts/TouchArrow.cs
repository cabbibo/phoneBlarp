using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.iOS;

public class TouchArrow : Game
{
    public Transform touchRep;
    public Collider collider;
    public Transform chaserRep;
    public Material touchMat;

    public GameObject EnemyPrefab;


    public GameObject lawn;
    public GameObject river;


    public Vector3 touchStart;
    public Vector3 touchCurrent;
    public Vector3 touchRelease;

    public Vector3 cPosition;
    public Vector3 cVelocity;
    public Vector3 cForce;

    public GameObject arrowPrefab;
    public GameObject enemyPrefab;
    public GameObject target;


    public LineRenderer lr;

    public bool holding;

    public List<GameObject> arrows;
    public List<GameObject> enemies;
    public float lastEnemyTime;

    public float lawnPosition;
    public float lawnSize;
    public float riverPosition;
    public float riverSize;

    public Collider lawnCollider;

    public override void OnAwake()
    {

        touchMat = touchRep.GetComponent<Renderer>().material;

        lawn.transform.position = new Vector3( 0 , 2,-screen.height * .3f );
        lawn.transform.localScale = new Vector3( screen.width , screen.height * .2f , 1);
    
        river.transform.position = new Vector3( 0 , 3f , -screen.height * .3f);
        river.transform.localScale = new Vector3( screen.width , screen.height * .1f , 1);
        
        lawnCollider = lawn.GetComponent<Collider>();
    }

    private Vector3 force;
    private GameObject currentArrow;
    void Update()
    {
        

        lawn.transform.position = new Vector3( 0 , .1f,-screen.height * lawnPosition );
        lawn.transform.localScale = new Vector3( screen.width , screen.height * lawnSize , 1);
    
        river.transform.position = new Vector3( 0 , .2f , -screen.height * riverPosition);
        river.transform.localScale = new Vector3( screen.width , screen.height * riverSize , 1);
    

        force = Vector3.zero;
        if (Input.GetMouseButtonDown(0))
        {

          
             //var ray = Camera.main.ScreenPointToRay (Input.mousePosition);
            //touchRep.position = ray.origin + ray.direction  * 2;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (lawnCollider.Raycast(ray, out hit, 100.0f))
            {
                SpawnArrow(hit.point);
                
            }

        }

        if (Input.GetMouseButton(0))
        {

             //var ray = Camera.main.ScreenPointToRay (Input.mousePosition);
            //touchRep.position = ray.origin + ray.direction  * 2;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (lawnCollider.Raycast(ray, out hit, 100.0f))
            {

                UpdateArrowDirection(hit.point);


            }

        }else{
        

          if( currentArrow != null ){
          if( holding == true ){

            currentArrow.GetComponent<Rigidbody>().AddForce(noY((touchStart - chaserRep.position)* 100) );
            chaserRep.position = currentArrow.transform.position;

          }

          if( (currentArrow.transform.position-touchStart).magnitude < .7f ){
            ReleaseArrow();
          }
        }

        }


        lr.SetPosition( 0 , touchRep.position-Camera.main.transform.forward * .2f);
        lr.SetPosition( 1 , chaserRep.position-Camera.main.transform.forward * .2f);


        int id = 0;
        int removeID = -1;
        foreach(GameObject arrow in arrows ){

          if( arrow != null ){

            arrow.GetComponent<ArrowInfo>().time -= .003f;
            if( arrow.GetComponent<ArrowInfo>().time < 0 ){ removeID = id; }
            arrow.transform.localScale = Vector3.one * arrow.GetComponent<ArrowInfo>().time;

            id ++;

          }

        }

        if( removeID >= 0 ){
          GameObject a = arrows[removeID];
          arrows.RemoveAt( removeID );
          DestroyImmediate( a );
        }

        if( playing ){
            UpdateEnemies();
        }

    }

    public Vector3 noY( Vector3 v ){
        return Vector3.Scale( v , -Vector3.left + Vector3.forward );
    }

    public void SpawnArrow( Vector3 location ){
        GameObject go = Instantiate( arrowPrefab );
        arrows.Add( go );
        currentArrow = go;
        go.GetComponent<ArrowInfo>().time = 1;

        currentArrow.GetComponent<Collider>().enabled = false;

        touchRep.position = location;
        touchStart = location;
        touchMat.color = new Color(1,0,0,1);

    }

    public void UpdateArrowDirection( Vector3 location ){
        chaserRep.position = location;
        touchCurrent = location;
        holding = true;
        currentArrow.transform.position = location;
    }

    public void ReleaseArrow(){
        currentArrow.GetComponent<Collider>().enabled = true;
        holding = false;
    }

    public void SpawnEnemy(){
        GameObject go = Instantiate( enemyPrefab );
        enemies.Add( go );
        go.transform.position = new Vector3( screen.width * Random.Range(-.5f, .5f) , 0 , screen.height * .45f );
        lastEnemyTime = Time.time;
    }

    public void UpdateEnemies(){

        if( Time.time-lastEnemyTime > 3 / (1 + .2f * (float)score) ){
            SpawnEnemy();
        }

        foreach( GameObject enemy in enemies ){
            enemy.GetComponent<Rigidbody>().AddForce( Vector3.forward * -3f );
        }

    }

    public void DestroyEnemy( GameObject g ){

        print( g );
        enemies.Remove( g );
        Destroy(g);
    }


    public override void DoRestart(){
      for( int i = 0; i < arrows.Count; i++ ){
        Destroy( arrows[i] );
      }
      arrows.Clear();
      for( int i = 0; i <enemies.Count; i++ ){
        Destroy(enemies[i] );
      }
     enemies.Clear();

    GameObject go = Instantiate( enemyPrefab );
    enemies.Add( go );
    go.transform.position = new Vector3( 0 , 0 , screen.height * .1f );
    lastEnemyTime = Time.time + 10000;

    }

    public override void DoStart(){
        lastEnemyTime = Time.time;
    }


}