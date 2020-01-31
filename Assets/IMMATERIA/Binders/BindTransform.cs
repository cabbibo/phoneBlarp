using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace IMMATERIA {
public class BindTransform : Binder
{
    public override void Bind(){
      toBind.BindMatrix("_Transform", () => transform.localToWorldMatrix);
      toBind.BindMatrix("_InverseTransform", () => transform.worldToLocalMatrix);
    }

}}