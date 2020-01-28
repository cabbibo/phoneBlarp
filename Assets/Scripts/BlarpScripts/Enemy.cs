using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public TouchBlarp game;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter( Collision c ){
      if( c.gameObject.tag == "arrow" && this.enabled ){
        game.Restart();
      }
    }

    void OnTriggerEnter( Collider c ){
      if( c.gameObject.tag == "arrow" && this.enabled ){
        game.Restart();
      }
    }


}