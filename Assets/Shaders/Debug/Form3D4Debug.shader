Shader "Debug/Form3D_4" {
    Properties {

    _Color ("Color", Color) = (1,1,1,1)
    _Size ("Size", float) = .01
    }


  SubShader{
    Cull Off
    Pass{
// inside SubShader
Tags { "Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True" }

// inside Pass
//ZWrite Off
//Blend SrcAlpha OneMinusSrcAlpha
      CGPROGRAM
      
      #pragma target 4.5

      #pragma vertex vert
      #pragma fragment frag

      #include "UnityCG.cginc"
#if defined( SHADER_API_D3D11 )
    struct Vert{
      float dist;
      float3 nor;
    };
#endif

#if defined( SHADER_API_METAL )       
   struct Vert{
      float dist;
      float3 nor;
    };
#endif


#if defined(SHADER_API_METAL)
  StructuredBuffer<Vert> _TransferBuffer;
#endif

#if defined(SHADER_API_D3D11) 
  StructuredBuffer<Vert> _TransferBuffer;
#endif


      uniform int _Count;
      uniform float _Size;
      uniform float3 _Color;

      uniform float3 _Dimensions;
      uniform float3 _Extents;
      uniform float3 _Center;
      


      uniform float4x4 _Transform;

      //A simple input struct for our pixel shader step containing a position.
      struct varyings {
          float4 pos      : SV_POSITION;
          float3 worldPos : TEXCOORD1;
          float3 nor  : TEXCOORD3;
          float2 uv       : TEXCOORD2;
      };

//Our vertex function simply fetches a point from the buffer corresponding to the vertex index
//which we transform with the view-projection matrix before passing to the pixel program.
varyings vert (uint id : SV_VertexID){

  varyings o;

  int base = id / 6;
  int alternate = id %6;

  if( base < _Count ){

      float3 extra = float3(0,0,0);

    float3 l = UNITY_MATRIX_V[0].xyz;
    float3 u = UNITY_MATRIX_V[1].xyz;
    
    float2 uv = float2(0,0);

    if( alternate == 0 ){ extra = -l - u; uv = float2(0,0); }
    if( alternate == 1 ){ extra =  l - u; uv = float2(1,0); }
    if( alternate == 2 ){ extra =  l + u; uv = float2(1,1); }
    if( alternate == 3 ){ extra = -l - u; uv = float2(0,0); }
    if( alternate == 4 ){ extra =  l + u; uv = float2(1,1); }
    if( alternate == 5 ){ extra = -l + u; uv = float2(0,1); }


      Vert v = _TransferBuffer[base % _Count];



    uint xID = base % _Dimensions.x;
    uint yID = (base / _Dimensions.x) % _Dimensions.y;
    uint zID = base / (_Dimensions.x * _Dimensions.y);

    float x = float(xID) / _Dimensions.x;
    float y = float(yID) / _Dimensions.y;
    float z = float(zID) / _Dimensions.z;


    // cell position

    float3 tmpPos = float3(x,y,z)-float3(.5 , .5 , .5);

    tmpPos *= 2;
    tmpPos *= _Extents;
    tmpPos += _Center;

    //tmpPos = mul( _Transform , float4(tmpPos ,1)).xyz;

      float3 pos = tmpPos;
      float3 fPos = mul(_Transform , float4(pos,1)).xyz;
      o.worldPos = (fPos) + extra * _Size / ((pow(v.dist,1) + .1) * 10);// (extra / v.dist) * _Size;
      o.uv = uv;
      o.nor = v.nor;
      o.pos = mul (UNITY_MATRIX_VP, float4(o.worldPos,1.0f));

  }
return o;
}
      //Pixel function returns a solid color for each point.
      float4 frag (varyings v) : COLOR {

          if( length( v.uv -.5) > .5 ){ discard;}
          
          return float4(v.nor * .5 + .5,1 );
      }

      ENDCG

    }
  }


}
