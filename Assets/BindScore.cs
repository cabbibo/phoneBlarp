using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IMMATERIA;

public class BindScore : Binder
{
  public Game game;

  public override void Bind(){
    toBind.BindInt("_Score",()=>game.score);
  }
}
