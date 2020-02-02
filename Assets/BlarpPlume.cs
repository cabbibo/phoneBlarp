using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IMMATERIA;

public class BlarpPlume : Cycle
{

    public FollowTrail trail;
    public int plumeSize;
    public int subPlumeSize;

    public Life plumeLife;
    public Life subPlumeLife;

    public Form plume;
    public Form subPlume;

    public override void Create(){

      SafeInsert( plumeLife );
      SafeInsert( subPlumeLife );

      SafeInsert( plume );
      SafeInsert( subPlume );

      plume.count = trail.form.count * plumeSize;
      subPlume.count = plume.count * subPlumeSize;

    }


    public override void Bind(){

      plumeLife.BindPrimaryForm("_VertBuffer",plume);
      subPlumeLife.BindPrimaryForm("_VertBuffer",subPlume);
      
      plumeLife.BindForm("_SkeletonBuffer",trail.form);
      subPlumeLife.BindForm("_SkeletonBuffer",plume);

      plumeLife.BindInt("_PlumeSize" , () => plumeSize );
      subPlumeLife.BindInt("_PlumeSize" , () => subPlumeSize );

    }




}
