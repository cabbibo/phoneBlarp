using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSchemeChanger : MonoBehaviour
{

    public Collider collider;

    public TouchBlarp game;

    public float spawnTime;
    public float spawnLength;

    public Renderer quad;// Material material;

    private float hitTime;
    public float startScale;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        

        float v = (Time.time - spawnTime) / spawnLength;
        if( v > 1 ){
          Despawn();
        }else{

          float dV = Mathf.Min( v * 10 , (1-v));
          transform.localScale = Vector3.one * dV * startScale;
        }
     
    }

    void OnTriggerEnter( Collider c ){
      if( c.gameObject.tag == "arrow" ){
        OnHit();
      }
    }

    public void OnHit(){
      game.ColorChangeHit();
       Despawn();
    }


    public void Despawn(){
      transform.position = Vector3.up * 11;
    }

    public void OnSpawn(){
      Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width * Random.Range(.1f,.9f), Screen.height * Random.Range(.1f,.9f), 0));
      RaycastHit hit;
      if (collider.Raycast(ray, out hit, 100.0f))
      {
        print( hit.point );
        transform.position = hit.point + Camera.main.transform.forward * -.2f;
        spawnTime = Time.time;
        quad.material.SetTexture("_ColorMap", game.aesthetics.colors[(game.aesthetics.colorScheme+1)%game.aesthetics.colors.Length]);
      }
    }


}