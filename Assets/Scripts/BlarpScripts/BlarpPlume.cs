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

    public TransferLifeForm plumeBody;
    public TransferLifeForm subPlumeBody;

    public override void Create(){


      plume.count = trail.form.count * plumeSize;
      subPlume.count = plume.count * subPlumeSize;

      SafeInsert( plume );
      SafeInsert( subPlume );

      SafeInsert( plumeLife );
      SafeInsert( subPlumeLife );


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
