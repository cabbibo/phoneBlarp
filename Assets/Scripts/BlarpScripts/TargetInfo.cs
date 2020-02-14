using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetInfo : MonoBehaviour
{

    public Collider collider;

    public TouchBlarp game;

    public float spawnTime;
    public float spawnLength;

    public Renderer quad;// Material material;

    public float hitTime;
    public float startScale;
    public float timeBetweenSpawns;

    public int spawned;
    public float spawnLerpVal;

    private Collider thisCollider;


    // Start is called before the first frame update
    void Awake()
    {
      thisCollider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        float v;

        v = (Time.time - hitTime) / timeBetweenSpawns;
        if( v > 1 && spawned == 0){
          OnSpawn();
        }

        v = (Time.time - spawnTime) / spawnLength;

        spawnLerpVal = v;
        if( v > 1 && spawned == 1 ){
          Despawn();
        }else{
          float dV = Mathf.Min( v * 10 , (1-v));
          transform.localScale = Vector3.one * dV * startScale;

          if( v > .1f ){
            thisCollider.enabled = true;
          }else{
            thisCollider.enabled = false;
          }
        }
     
    }

    void OnTriggerEnter( Collider c ){
      if( c.gameObject.tag == "arrow" ){
        OnHit();
      }

      if( c.gameObject.tag == "Enemy"){
        game.OnEnemyCollect();
        Despawn();
      }
    }

    public void OnHit(){
      Despawn();
      game.Next();
    }

    public void Restart(){
      Despawn();
    }

    public void Despawn(){
      if( thisCollider == null ){ thisCollider = GetComponent<Collider>(); }
      thisCollider.enabled = false;
      hitTime = Time.time;
      spawned = 0;
      transform.position = Vector3.up * 11;
    }

    public void OnSpawn(){
      spawned = 1;
      Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width * Random.Range(.1f,.9f), Screen.height * Random.Range(.1f,.9f), 0));
      RaycastHit hit;
      if (collider.Raycast(ray, out hit, 100.0f))
      {

        if( game.inMenu){
          print("IN MENNU");
          transform.position = spawnInMenu();
        }else{
          transform.position = hit.point + Camera.main.transform.forward * -.2f;
        }

        spawnTime = Time.time;
      }else{
        print("SPAWNING INCORRECTIO!");
      }
    }

    public Vector3 spawnInMenu(){

      float x;
      if( game.blarp.transform.position.x > 0 ){
        x = Random.Range( .2f , .4f );
      }else{
        x = Random.Range( .6f , .8f );
      }

      float y;
      if( game.blarp.transform.position.z > 0 ){
        y = Random.Range( .2f , .4f );
      }else{
        y = Random.Range( .6f , .7f );
      }

      Ray ray = Camera.main.ScreenPointToRay(new Vector3(x * Screen.width,y* Screen.height, 0));
      RaycastHit hit;
      Vector3 position = Vector3.zero;
      if (collider.Raycast(ray, out hit, 100.0f))
      {
        position = hit.point;
      }else{
        print("SPAWNING INCORRECTIO!");
      }

      return position;

    }


}
