using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IMMATERIA{
public class BindPrimaryForm : Binder
{
    
  public Form form;
  public string name;

  public override void Bind(){
    toBind.BindPrimaryForm( name , form );
  }

}}
