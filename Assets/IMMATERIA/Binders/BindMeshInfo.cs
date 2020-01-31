using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IMMATERIA{
public class BindMeshInfo : Binder
{
  public Form verts;
  public IndexForm tris;

  public override void Bind(){

    toBind.BindForm("_VertBuffer" , verts);
    toBind.BindForm("_TriBuffer" , tris);

  }

}}
