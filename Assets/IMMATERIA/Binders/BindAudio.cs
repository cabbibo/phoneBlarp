using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IMMATERIA;

public class BindAudio : Binder
{

    public AudioListenerTexture audio;

    public override void Bind(){
      toBind.BindTexture( "_AudioMap" , () => audio.texture );
      toBind.BindForm( "_AudioBuffer" , audio );
    }
}
