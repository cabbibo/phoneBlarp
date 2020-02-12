using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerWallHit : MonoBehaviour
{
    public Game game;
    
    public void OnCollisionEnter( Collision c){
      game.walls.SetWallCollision( c.contacts[0].point , c.relativeVelocity.magnitude );
    }
}
