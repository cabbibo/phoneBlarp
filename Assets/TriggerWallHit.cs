using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerWallHit : MonoBehaviour
{
    public TouchBlarp game;
    
    public void OnCollisionEnter( Collision c){
      game.SetWallCollision( c.contacts[0].point );
    }
}
