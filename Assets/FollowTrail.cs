using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IMMATERIA;

public class FollowTrail : Simulation
{

  public Transform leader;
  public Game game;
  public TransferLifeForm transferForm;

  public override void Create(){
    SafeInsert( transferForm );
  }

  public override void Bind(){

    life.BindVector3( "_Leader" , ()=> leader.position );
    life.BindInt( "_Score" , ()=> game.score );

    print( leader );
    print(transferForm );
    print( life );
    print( transferForm.transfer );


    transferForm.transfer.BindVector3( "_Leader" , ()=> leader.position );
    transferForm.transfer.BindInt( "_Score" , ()=> game.score );


  }

}
