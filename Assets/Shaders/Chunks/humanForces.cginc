struct Human {
  float4x4 leftHand;
  float4x4 rightHand;
  float4x4 head;
  float leftTrigger;
  float rightTrigger;
  float voice;
  float debug;
};

RWStructuredBuffer<Human> _HumanBuffer;
int _HumanBuffer_COUNT;

float _HumanRadius;
float _HumanForce;



float3 HumanForces(float3 p){
  float3 totalForce;
  
  for( int i = 0; i < _HumanBuffer_COUNT; i++ ){
    Human h = _HumanBuffer[i];

    float3 hL = mul(h.leftHand,float4(0,0,0,1));
    float3 hR = mul(h.rightHand,float4(0,0,0,1));
    float3 hH = mul(h.head,float4(0,0,0,1));

    float3 d;

    d = p - hL;

    if( length( d ) < _HumanRadius && length( d ) > .001 ){ 
      totalForce += normalize( d ) / length(d);
    }


     d = p - hR;
    if( length( d ) < _HumanRadius && length( d ) > .001 ){ 
      totalForce += normalize( d ) / length(d);
    }


    d = p - hH;
    if( length( d ) < _HumanRadius && length( d ) > .001 ){ 
      totalForce += normalize( d ) / length(d);
    }

    

  }
totalForce *= _HumanForce;
  return mul( _FullWTL , float4( totalForce * _HumanForce,0)).xyz;
}