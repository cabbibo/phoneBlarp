using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IMMATERIA;

public class BindPlumeData : Binder
{
  

  public Form parent;
  public int plumeSize;

  public override void Bind(){
    toBind.BindForm("_ParentBuffer", parent);
    toBind.BindInt("_PlumeSize", () => plumeSize );
  }
}
