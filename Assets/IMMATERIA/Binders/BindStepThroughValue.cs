using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace IMMATERIA{
public class BindStepThroughValue : Binder
{

  public IndexForm toStepThrough;
  public int currentStep;
  public float percentageDone;

  public int iterationsPerFrame;
  public int stepsPerIteration;

  public override void OnBirthed(){
    currentStep = 0;
  }

  public override void Bind(){
    toBind.BindInt( "_CurrentStep" , () => currentStep );
    toBind.BindFloat( "_PercentageDone" , () => percentageDone );
  }

  public override void WhileLiving( float v ){


    for( int i = 0; i < iterationsPerFrame; i++ ){
      percentageDone = (float)currentStep / (float)toStepThrough.count;
      currentStep += stepsPerIteration;
      toBind.YOLO();
    }

  }

}}
