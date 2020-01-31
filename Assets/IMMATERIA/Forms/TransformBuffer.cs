using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IMMATERIA {
public class TransformBuffer : Form
{

  public Transform[] transforms;
  public bool dynamic;
  [HideInInspector]public float[] values;
  private float[] tmpVals;

  public override void SetStructSize(){ structSize = 32; }
  public override void SetCount(){ count = transforms.Length; }

  public override void Embody(){
    values = new float[ count * structSize ];
    tmpVals = new float[ 16 ];
    SetInfo();
  }
  public void SetInfo(){
  
    for( int i = 0; i < transforms.Length; i++ ){
      if( transforms[i] != null ){
      tmpVals = HELP.GetMatrixFloats(transforms[i].localToWorldMatrix);
      for( int j = 0; j < 16; j++ ){
        values[i * 32 + j ] = tmpVals[j];
      }

      tmpVals = HELP.GetMatrixFloats(transforms[i].worldToLocalMatrix);
      for( int j = 0; j < 16; j++ ){
        values[i * 32 + j + 16 ] = tmpVals[j];
      }
      }else{
    tmpVals = HELP.GetMatrixFloats(transform.localToWorldMatrix);
      for( int j = 0; j < 16; j++ ){
        values[i * 32 + j ] = tmpVals[j];
      }

      tmpVals = HELP.GetMatrixFloats(transform.worldToLocalMatrix);
      for( int j = 0; j < 16; j++ ){
        values[i * 32 + j + 16 ] = tmpVals[j];
      }

      }
    }

    SetData(values);
  }


  public override void WhileLiving( float v ){

    if( dynamic && active ){
      SetInfo();
    }
  }

  public void Remake( List<Transform> newTransforms ){
    transforms = newTransforms.ToArray();

     Reset();
    _Destroy(); 
    _Create(); 
    _OnGestate();
    _OnGestated();
    _OnBirth(); 
    _OnBirthed();

    _OnLive(); 


  }
}
}