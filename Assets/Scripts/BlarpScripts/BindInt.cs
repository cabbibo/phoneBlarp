﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IMMATERIA;

public class BindInt : Binder
{
    public int value;
    public string nameInShader;

    public override void Bind(){
      toBind.BindInt(nameInShader,()=>value);
    }
}
