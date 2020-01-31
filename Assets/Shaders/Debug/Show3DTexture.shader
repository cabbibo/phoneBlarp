Shader "Debug/Show3DTexture" {


  Properties {

        // This is how many steps the trace will take.
        // Keep in mind that increasing this will increase
        // Cost
    _NumberSteps( "Number Steps", Int ) = 3

    // Total Depth of the trace. Deeper means more parallax
    // but also less precision per step
    _TotalDepth( "Total Depth", Float ) = 0.16


    _PatternSize( "Pattern Size", Float ) = 10
    _HueSize( "Hue Size", Float ) = .3
    _BaseHue( "Base Hue", Float ) = .3

    _MainTex("", 3D) = "white" {}




  }

  SubShader {


    Pass {

      CGPROGRAM


      #pragma target 4.5
      #pragma vertex vert
      #pragma fragment frag

      #include "UnityCG.cginc"
     #include "../Chunks/noise.cginc"
    struct SDF {
  float  dist;
  float3 nor;
  float  ao;
  float  pn;
  float2 debug;
};

      uniform int _NumberSteps;
      uniform float _TotalDepth;
      uniform float _PatternSize;
      uniform float _HueSize;
      uniform float _BaseHue;

      uniform sampler3D _MainTex;


uniform float3 _Dimensions;
uniform float3 _Extents;
uniform float3 _Center;

uniform float4x4 _FormTransform;


StructuredBuffer<SDF> _volumeBuffer;

      struct VertexIn{
         float4 position  : POSITION;
         float3 normal    : NORMAL;
         float4 texcoord  : TEXCOORD0;
         float4 tangent   : TANGENT;
      };


      struct VertexOut {
          float4 pos        : POSITION;
          float3 normal     : NORMAL;
          float4 uv         : TEXCOORD0;
          float3 ro         : TEXCOORD1;
          float3 rd         : TEXCOORD2;
      };


            float3 hsv(float h, float s, float v){
        return lerp( float3( 1.0,1,1 ), clamp(( abs( frac(h + float3( 3.0, 2.0, 1.0 ) / 3.0 )
                             * 6.0 - 3.0 ) - 1.0 ), 0.0, 1.0 ), s ) * v;
      }

      float getFogVal( float3 pos ){

        return abs( sin( pos.x * _PatternSize ) + sin(pos.y * _PatternSize ) + sin( pos.z * _PatternSize ));
      }

      VertexOut vert(VertexIn v) {

        VertexOut o;

        o.normal = v.normal;

        o.uv = v.texcoord;



        float3 fPos = v.position;
        // Getting the position for actual position
        o.pos = UnityObjectToClipPos(  fPos );



        float3 mPos = mul( unity_ObjectToWorld , float4(fPos,1) ).xyz;

        // The ray origin will be right where the position is of the surface
        o.ro = mPos;//fPos;


        float3 camPos = mul( unity_WorldToObject , float4( _WorldSpaceCameraPos , 1. )).xyz;

        // the ray direction will use the position of the camera in local space, and
        // draw a ray from the camera to the position shooting a ray through that point
        o.rd = normalize(mPos - _WorldSpaceCameraPos);//normalize( fPos.xyz - camPos );

        return o;

      }





      // Fragment Shader
      fixed4 frag(VertexOut v) : COLOR {

                // Ray origin
        float3 ro           = v.ro;

        // Ray direction
        float3 rd           = normalize(v.ro - _WorldSpaceCameraPos);

        // Our color starts off at zero,
        float3 col = float3( 0.0 , 0.0 , 0.0 );



        float3 p;
        float depth = 0;
        float dist = 0;

        float hit = 0;

        float3 tmpPos = mul( _FormTransform ,float4(ro,1));


        float4 info = tex3D(_MainTex, (((tmpPos-_Center) / _Extents) + 1)/2   );

        //col = float3(sin( id / 100),0,0);
        col = info.yzw* .5 + .5;//*100;

        //col *= info.x*3;


            fixed4 color;
        color = fixed4( col / (1 + info.x) , 1. );
        return color;
      }

      ENDCG
    }
  }
  FallBack "Diffuse"
}
