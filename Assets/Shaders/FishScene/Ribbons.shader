Shader "Scenes/FishScene/Ribbons1" {
  Properties {

    _Color ("Color", Color) = (1,1,1,1)
    _FalloffRadius ("Falloff", float) = 20

    _MainTex ("Texture", 2D) = "white" {}
    _BumpMap ("Bumpy", 2D) = "white" {}

    _ColorStart( "ColorStart" , float ) = .2
    _ColorWidth( "ColorWidth" , float ) = .4
    _OutlineColor( "OutlineColor" , float ) = .4


       _PLightMap1 ("PLightMap1", 2D) = "white" {}
       _PLightMap2 ("PLightMap2", 2D) = "white" {}
       _PLightMap3 ("PLightMap3", 2D) = "white" {}
       _PLightMap4 ("PLightMap4", 2D) = "white" {}
       _PLightMap5 ("PLightMap5", 2D) = "white" {}
       _ColorMap ("ColorMap", 2D) = "white" {}
  }

    SubShader {
        // COLOR PASS

        Pass {

          // Lighting/ Texture Pass
Stencil
{
Ref 6
Comp always
Pass replace
ZFail keep
}
            Tags{ "LightMode" = "ForwardBase" }
            Cull Off

            CGPROGRAM
            #pragma target 4.5
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight

            #include "UnityCG.cginc"
            #include "AutoLight.cginc"

      #include "../Chunks/hsv.cginc"
            #include "../Chunks/hash.cginc"


  struct Vert{
      float3 pos;
      float3 vel;
      float3 nor;
      float3 tan;
      float2 uv;
      float2 debug;
    };

  StructuredBuffer<Vert> _VertBuffer;
  StructuredBuffer<int> _TriBuffer;


  float _ColorStart;
  float _ColorWidth;

      float3 _Color;
      float3 _PlayerPosition;
      float _FalloffRadius;
      sampler2D _MainTex;
      sampler2D _ColorMap;
      sampler2D _BumpMap;


            struct varyings {
                float4 pos      : SV_POSITION;
                float3 nor      : TEXCOORD0;
                float2 uv       : TEXCOORD1;
                float3 eye      : TEXCOORD5;
                float3 worldPos : TEXCOORD6;
        float3 debug    : TEXCOORD7;
                float3 vel    : TEXCOORD9;
                float3 closest    : TEXCOORD8;
                   half3 tspace0 : TEXCOORD11; // tangent.x, bitangent.x, normal.x
                half3 tspace1 : TEXCOORD12; // tangent.y, bitangent.y, normal.y
                half3 tspace2 : TEXCOORD13; // tangent.z, bitangent.z, normal.z
                half3 tang : TEXCOORD14; // tangent.z, bitangent.z, normal.z
                UNITY_SHADOW_COORDS(2)
            };


            sampler2D _PLightMap1;
            sampler2D _PLightMap2;
            sampler2D _PLightMap3;
            sampler2D _PLightMap4;
            sampler2D _PLightMap5;
            varyings vert(uint id : SV_VertexID) {

                   Vert v = _VertBuffer[_TriBuffer[id]];
        
        float3 fPos   = v.pos;
        float3 fNor   = v.nor;
        float3 fVel   = v.vel;
        float2 fUV    = v.uv;
        float2 debug  = v.debug;

                varyings o;

                UNITY_INITIALIZE_OUTPUT(varyings, o);

                o.pos = mul(UNITY_MATRIX_VP, float4(fPos,1));
                o.worldPos = fPos;
                o.eye = _WorldSpaceCameraPos - fPos;
                o.nor = v.tan;//-normalize(cross(v.nor , v.tan));
                o.vel = fVel;

                float offset = floor(hash(debug.x) * 6) /6;
                o.uv =  fUV * float2(1,1./6.) + float2(-.1,offset);
                o.debug = float3(debug.x,debug.y,0);

                o.tang = v.tan;

                half3 wNormal = v.nor;
                half3 wTangent = v.tan;
                // compute bitangent from cross product of normal and tangent
                //half tangentSign = tangent.w * unity_WorldTransformParams.w;
                half3 wBitangent = cross(wNormal, wTangent);// * tangentSign;
                // output the tangent space matrix
                o.tspace0 = half3(wTangent.x, wBitangent.x, wNormal.x);
                o.tspace1 = half3(wTangent.y, wBitangent.y, wNormal.y);
                o.tspace2 = half3(wTangent.z, wBitangent.z, wNormal.z);

                UNITY_TRANSFER_SHADOW(o,o.worldPos);

                return o;
            }

            float4 frag(varyings v) : COLOR {



              float4 color = float4(0,0,0,0);// = tex2D(_MainTex,v.uv );
              float4 tCol = tex2D(_MainTex,v.uv );


 // sample the normal map, and decode from the Unity encoding
                half3 tnormal =UnpackNormal(tex2D(_BumpMap, v.uv));// lerp( i.norm ,  , specMap.x);
                // transform normal from tangent to world space
                half3 worldNormal;
                worldNormal.x = dot(v.tspace0, tnormal);
                worldNormal.y = dot(v.tspace1, tnormal);
                worldNormal.z = dot(v.tspace2, tnormal);

               worldNormal = lerp( v.nor , worldNormal , tCol.x);
          half3 worldViewDir = normalize(UnityWorldSpaceViewDir(v.worldPos));
                //half3 worldRefl = reflect(-worldViewDir, worldNormal);
                half3 worldRefl = refract(worldViewDir, worldNormal,.8);
                half4 skyData = UNITY_SAMPLE_TEXCUBE(unity_SpecCube0, worldRefl);
                half3 skyColor = DecodeHDR (skyData, unity_SpecCube0_HDR);


                float4 p1 = tex2D( _PLightMap1 , v.uv * float2(.1,3.8)*2 );
                float4 p2 = tex2D( _PLightMap2 , v.uv * float2(.1,3.8)*2 );
                float4 p3 = tex2D( _PLightMap3 , v.uv * float2(.1,3.8)*2 );
                float4 p4 = tex2D( _PLightMap4 , v.uv * float2(.1,3.8)*2 );
                float4 p5 = tex2D( _PLightMap5 , v.uv * float2(.1,3.8)*2 );

                float3 fNor = v.nor;

                fNor = v.tang;//normalize(cross(normalize(v.vel* 100), v.nor));
                float m = dot(_WorldSpaceLightPos0.xyz , fNor);

                float fern = dot( _WorldSpaceLightPos0, normalize(fNor) );
 
                //m = 1-1.5*fern;//1-pow(fern,.7);//*fern*fern;//pow( fern * fern, 1);
                //m = saturate( 1-m );
                //m = 5 * m;

                m =  3-3*( .8*UNITY_SHADOW_ATTENUATION(v,v.worldPos));

                float4 fLCol = float4(1,0,0,1);
                if( m < 1 ){
                    fLCol = lerp( p1 , p2 , saturate(m) );
                }else if( m >= 1 && m < 2){
                    fLCol = lerp( p2 , p3 , m-1 );
                }else if( m >= 2 && m < 3){
                    fLCol = lerp( p3 , p4 , m-2 );
                }else if( m >= 3 && m < 4){
                    fLCol = lerp( p4 , p5 , m-3 );
                }else if( m >= 4 && m < 5){
                    fLCol = lerp( p5 , p5 , m-4 );
                }else{
                    fLCol = p5;
                }


              float4 cCol = tex2D(_ColorMap,float2(tCol.x * _ColorWidth + _ColorStart,0) );
        
            //  fixed shadow = UNITY_SHADOW_ATTENUATION(v,v.worldPos  ) * .7 + .3 ;
              
              color.xyz =(fLCol * .8 + .2) * cCol;//skyColor  * cCol ;// * tCol;;//worldNormal * .5 + .5;//tCol;
             // color =  float4(v.nor * .5 + .5,1);//v.uv.x;
              if( tCol.a < .3 ){ discard; }    
              return float4( color.xyz, 1.);
            }

            ENDCG
        }


   // SHADOW PASS

    Pass
    {
      Tags{ "LightMode" = "ShadowCaster" }


      Fog{ Mode Off }
      ZWrite On
      ZTest LEqual
      Cull Off
      Offset 1, 1
      CGPROGRAM

      #pragma target 4.5
      #pragma vertex vert
      #pragma fragment frag
      #pragma multi_compile_shadowcaster
      #pragma fragmentoption ARB_precision_hint_fastest

  #include "UnityCG.cginc"

            #include "../Chunks/hash.cginc"

  struct Vert{
      float3 pos;
      float3 vel;
      float3 nor;
      float3 tan;
      float2 uv;
      float2 debug;
    };

  StructuredBuffer<Vert> _VertBuffer;
  StructuredBuffer<int> _TriBuffer;

      #include "../Chunks/ShadowCasterPos.cginc"
sampler2D _MainTex;
  struct v2f {
        V2F_SHADOW_CASTER;
        float2 uv : TEXCOORD1;
        float2 debug : TEXCOORD2;
      };


      v2f vert( uint id : SV_VertexID)
      {

        Vert v = _VertBuffer[_TriBuffer[id]];
        
        float3 fPos   = v.pos;
        float3 fNor   = v.nor;
        float2 fUV    = v.uv;
        float2 debug  = v.debug;

        v2f o;
       
        float offset = floor(hash(debug.x) * 6) /6;
               o.uv =  fUV * float2(1,1./6.) + float2(-.1,offset);
        //o.uv = fUV.xy  -float2(0.1,0);// *float2(1./6.,1);;
        float4 position = ShadowCasterPos(v.pos, -v.tan);
        o.pos = UnityApplyLinearShadowBias(position);
        o.debug = debug;
        return o;
      }

      float4 frag(v2f i) : COLOR
      {
        float4 col = tex2D(_MainTex,i.uv);

        //if( i.debug.y < .3 ){ discard; }
        if( col.a < .3){discard;}
        SHADOW_CASTER_FRAGMENT(i)
      }
      ENDCG
    }
  
   Pass
    {

// Outline Pass
Cull OFF
ZWrite OFF
ZTest ON
Stencil
{
Ref 6
Comp notequal
Fail keep
Pass replace
}
      
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 4.5
            // make fog work
            #pragma multi_compile_fogV
 #pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight

      #include "UnityCG.cginc"
      #include "AutoLight.cginc"
    


            #include "../Chunks/Struct16.cginc"
            #include "../Chunks/hash.cginc"


            struct v2f { 
              float4 pos : SV_POSITION; 
              float2 uv : TEXCOORD1; 
            };
            float4 _Color;
            float _OutlineColor;

            sampler2D _MainTex;

            StructuredBuffer<Vert> _VertBuffer;
            StructuredBuffer<int> _TriBuffer;

            v2f vert ( uint vid : SV_VertexID )
            {
                v2f o;

              int id = _TriBuffer[vid];
              id %= 2;
                Vert v = _VertBuffer[_TriBuffer[vid]];
                float3 fPos = v.pos + normalize(v.nor) * (float(id)-.5)* .01;
                o.pos = mul (UNITY_MATRIX_VP, float4(fPos,1.0f));
                float offset = floor(hash(v.debug.x) * 6) /6;
                o.uv =  v.uv * float2(1,1./6.) + float2(-.1,offset);

             
                return o;
            }

            sampler2D _ColorMap;
            fixed4 frag (v2f v) : SV_Target
            {
              
              float4 tCol = tex2D(_MainTex,v.uv );
              if( tCol.a < .3 ){ discard; } 
                fixed4 col = 0;//1;//tex2D(_ColorMap, float2( _OutlineColor ,0));//float4(1,0,0,1);
                return col;
            }

            ENDCG
        }


    }

}
