using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IMMATERIA;

public class BindTailSize : Binder
{
  public TouchBlarp game;

  public override void Bind(){
    toBind.BindInt("_Score",()=>game.tailSize);
  }
}
