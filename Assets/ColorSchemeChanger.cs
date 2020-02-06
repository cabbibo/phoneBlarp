using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSchemeChanger : MonoBehaviour
{

    public Collider collider;

    public Game game;

    public float spawnTime;
    public float spawnLength;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if( Time.time - spawnTime > spawnLength ){
          transform.position = Vector3.one * 1000;
        }
     
    }

    void OnTriggerEnter( Collider c ){

      print(c);

      if( c.gameObject.tag == "arrow" ){
        newLocation();
      }
    }

    public void OnHit(){

    }

    public void OnSpawn(){
      Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width * Random.Range(.1f,.9f), Screen.height * Random.Range(.1f,.9f), 0));
      RaycastHit hit;
      if (collider.Raycast(ray, out hit, 100.0f))
      {
        print( hit.point );
        transform.position = hit.point + Camera.main.transform.forward * -.2f;
        spawnTime = Time.time
      }
    }

    public void newLocation(){

      //transform.position = Vector3.left * 1000;
      hitTime = Time.time;
      needsNewLocation = true;
        

          needsNewLocation = false;



      game.Next();

    }

}