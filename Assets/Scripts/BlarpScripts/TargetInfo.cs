using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetInfo : MonoBehaviour
{

    public Collider collider;

    public Game game;

    public float hitTime;
    public bool needsNewLocation;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if( Time.time - hitTime > .4f  && needsNewLocation == true){
          Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width * Random.Range(.1f,.9f), Screen.height * Random.Range(.1f,.9f), 0));
          RaycastHit hit;
          if (collider.Raycast(ray, out hit, 100.0f))
          {
            print( hit.point );
            transform.position = hit.point + Camera.main.transform.forward * -.2f;
          }

          needsNewLocation = false;

        }
    }

    void OnTriggerEnter( Collider c ){

      print(c);

      if( c.gameObject.tag == "arrow" ){
        newLocation();
      }
    }

    public void newLocation(){

      transform.position = Vector3.left * 1000;
      hitTime = Time.time;
      needsNewLocation = true;


      game.Next();

    }

}
