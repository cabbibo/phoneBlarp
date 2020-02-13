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
        transform.position = hit.point + Camera.main.transform.forward * -.2f;
        spawnTime = Time.time;
      }else{
        print("SPAWNING INCORRECTIO!");
      }
    }


}
