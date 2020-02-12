using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowEnemy : MonoBehaviour
{

    public TouchArrow game;

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
        print("NEXT");
        game.DestroyEnemy( gameObject);
        game.DestroyArrow( c.gameObject );
        game.Next();
      }
      if( c.gameObject.tag == "lawn" && this.enabled ){
        print("HELLLLLLOO");
        game.Restart();
      }
    }

    void OnTriggerEnter( Collider c ){
      if( c.gameObject.tag == "arrow" && this.enabled ){
        print("NEXT");
        game.DestroyEnemy( gameObject );
        game.Next();
      }
      if( c.gameObject.tag == "lawn" && this.enabled ){
        print("HELLLLLLOO");
        game.Restart();
      }
    }


}