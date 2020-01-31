// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Scenes/SceneShader"
{
        Properties {

       _ColorMap ("ColorMap", 2D) = "white" {}
       _TextureMap ("TextureMap", 2D) = "white" {}
       _NormalMap ("NormalMap", 2D) = "white" {}
       _ShinyMap ("ShinyMap", 2D) = "white" {}

       
        _PLightMap("Painterly Light Map", 2D) = "white" {}
    _CubeMap( "Cube Map" , Cube )  = "defaulttexture" {}

        _ColorStart("_ColorStart",float) = 0
        _ColorRandomSize("_ColorRandomSize",float) = 0
        _ColorSize("_ColorSize",float) = 0
        _Saturation("_Saturation",float) = .3
        _Brightness("_Brightness",float) = .1
    
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Cull Off
        Pass
        {

          Stencil
{
Ref 9
Comp always
Pass replace
ZFail keep
}
             Tags {"LightMode"="ForwardBase"}
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #pragma target 4.5
            #include "UnityCG.cginc"
            #include "Lighting.cginc"

            // compile shader into multiple variants, with and without shadows
            // (we don't care about any lightmaps yet, so skip these variants)
            //#pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight

 #pragma multi_compile_fwdbase
            // shadow helper functions and macros
            #include "AutoLight.cginc"

            struct v2f {
                float3 worldPos : TEXCOORD0;
                // these three vectors will hold a 3x3 rotation matrix
                // that transforms from tangent to world space
                half3 tspace0 : TEXCOORD1; // tangent.x, bitangent.x, normal.x
                half3 tspace1 : TEXCOORD2; // tangent.y, bitangent.y, normal.y
                half3 tspace2 : TEXCOORD3; // tangent.z, bitangent.z, normal.z
                // texture coordinate for the normal map
                float2 uv : TEXCOORD4;
                float4 pos : SV_POSITION;

                fixed3 ambient : COLOR1;

              // in v2f struct;
LIGHTING_COORDS(5,6)
            };

            float3 _TouchLocation;
            float  _TouchPower;

            // vertex shader now also needs a per-vertex tangent vector.
            // in Unity tangents are 4D vectors, with the .w component used to
            // indicate direction of the bitangent vector.
            // we also need the texture coordinate.
            v2f vert (float4 vertex : POSITION, float3 normal : NORMAL, float4 tangent : TANGENT, float2 uv : TEXCOORD0)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(vertex);
                o.worldPos = mul(unity_ObjectToWorld, vertex).xyz;
                half3 wNormal = UnityObjectToWorldNormal(normal);
                half3 wTangent = UnityObjectToWorldDir(tangent.xyz);
                // compute bitangent from cross product of normal and tangent
                half tangentSign = tangent.w * unity_WorldTransformParams.w;
                half3 wBitangent = cross(wNormal, wTangent) * tangentSign;
                // output the tangent space matrix
                o.tspace0 = half3(wTangent.x, wBitangent.x, wNormal.x);
                o.tspace1 = half3(wTangent.y, wBitangent.y, wNormal.y);
                o.tspace2 = half3(wTangent.z, wBitangent.z, wNormal.z);
                o.uv = uv;


                o.ambient = ShadeSH9(half4(wNormal,1));

            UNITY_TRANSFER_SHADOW(o,o.worldPos);
                return o;
            }

            // normal map texture from shader properties
            sampler2D _NormalMap;
            sampler2D _TextureMap;
            sampler2D _ShinyMap;
            sampler2D _ColorMap;
        
            float _ColorSize;
            float _ColorStart;
            fixed4 frag (v2f i) : SV_Target
            {
                // sample the normal map, and decode from the Unity encoding
                half3 tnormal = UnpackNormal(tex2D(_NormalMap, i.uv));
                half3 color = tex2D(_TextureMap, i.uv);
                half3 shiny = tex2D(_ShinyMap, i.uv);
                // transform normal from tangent to world space
                half3 worldNormal;
                worldNormal.x = dot(i.tspace0, tnormal);
                worldNormal.y = dot(i.tspace1, tnormal);
                worldNormal.z = dot(i.tspace2, tnormal);

                float attenuation = UNITY_SHADOW_ATTENUATION(i,i.worldPos);
                fixed atten = .5+.5*attenuation;
                float m = dot( worldNormal , _WorldSpaceLightPos0 );
                m *= atten;
                float3 c2 = tex2D(_ColorMap, float2(m  * _ColorSize  + _ColorStart,0));
                // rest the same as in previous shader
                half3 worldViewDir = normalize(UnityWorldSpaceViewDir(i.worldPos));
                half3 worldRefl = reflect(-worldViewDir, worldNormal);
                half4 skyData = UNITY_SAMPLE_TEXCUBE(unity_SpecCube0, worldRefl);
                half3 skyColor = DecodeHDR (skyData, unity_SpecCube0_HDR);
                fixed4 c = 0;
                c.rgb = lerp(m*m * color * c2 + i.ambient * atten ,skyColor  * color  , shiny.x) ;
                return c;
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
Ref 9
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
    


               struct Vert{
      float3 pos;
      float3 vel;
      float3 nor;
      float3 tan;
      float2 uv;
      float2 debug;
    };


            struct v2f { 
              float4 pos : SV_POSITION; 
            };
            float4 _Color;

  struct appdata
            {
                float4 pos : POSITION;
                float3 nor : NORMAL;
                float2 uv : TEXCOORD0;
            };
            v2f vert ( appdata v )
            {
                v2f o;

        
                float3 fPos = v.pos + v.nor * .005;
                o.pos = mul (UNITY_MATRIX_VP, float4(fPos,1.0f));


                return o;
            }

            sampler2D _ColorMap;
            fixed4 frag (v2f v) : SV_Target
            {
              
                fixed4 col = 0;//1;//tex2D(_ColorMap, float2( .8,0));
                return col;
            }

            ENDCG
        }

    
  

  
  
    }

FallBack "diffuse"
}
