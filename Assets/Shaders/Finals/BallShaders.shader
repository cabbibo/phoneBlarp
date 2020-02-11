Shader "Outlined/Uniform"
{
    Properties
    {
        _Color("Main Color", Color) = (0.5,0.5,0.5,1)
        _MainTex ("Texture", 2D) = "white" {}
        _OutlineColor ("Outline color", Color) = (0,0,0,1)
        _OutlineWidth ("Outlines width", Range (0.0, 2.0)) = 1.1
    _CubeMap( "Cube Map" , Cube )  = "defaulttexture" {}
    }

    CGINCLUDE
    #include "UnityCG.cginc"



    uniform float _OutlineWidth;
    uniform float4 _OutlineColor;
    uniform sampler2D _MainTex;
    uniform float4 _Color;
    uniform sampler2D _GlobalColorMap;
    uniform float3 _Velocity;

    ENDCG

    SubShader
    {
        

    

      

        Pass //Outline
        {

             Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" }
            ZWrite Off
            Cull Back
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

                struct appdata
    {
        float4 vertex : POSITION;
        float3 normal : NORMAL;
    };

    struct v2f
    {
        float4 pos : POSITION;
    };

            v2f vert(appdata v)
            {
                appdata original = v;

                 float3 world = mul( unity_ObjectToWorld, float4( v.vertex.xyz,1 ));
                float3 wNorm = mul( unity_ObjectToWorld, float4( v.normal,0 ));
                if( length( _Velocity ) > .01 ){
                    world += (-_Velocity * .04 + wNorm * dot( normalize(_Velocity) , wNorm ) * .1 + _Velocity * .03 * dot(normalize(_Velocity),wNorm )) * clamp( length(_Velocity) * 10, 0 ,1);
                }

                world += _OutlineWidth * wNorm;
                v.vertex.xyz += _OutlineWidth * normalize(v.vertex.xyz);

                v2f o;
                o.pos = mul(UNITY_MATRIX_VP,float4(world,1));//v.vertex);
                return o;

            }

            half4 frag(v2f i) : COLOR
            {
                return _OutlineColor;
            }

            ENDCG
        }

         // shadow caster rendering pass, implemented manually
        // using macros from UnityCG.cginc
        Pass
        {
            Tags {"LightMode"="ShadowCaster"}

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_shadowcaster
            

            struct v2f { 
                V2F_SHADOW_CASTER;
                float2 uv : TEXCOORD0;
            };

            v2f vert(appdata_base v)
            {

                  appdata_base original = v;

                float3 world = mul( unity_ObjectToWorld, float4( v.vertex.xyz,1 ));
                float3 wNorm = mul( unity_ObjectToWorld, float4( v.normal,0 ));
                if( length( _Velocity ) > .01 ){
                    world += (-_Velocity * .04 + wNorm * dot( normalize(_Velocity) , wNorm ) * .1 + _Velocity * .03 * dot(normalize(_Velocity),wNorm )) * clamp( length(_Velocity) * 10, 0 ,1);
                }

                world += _OutlineWidth * wNorm;
                v.vertex.xyz += _OutlineWidth * normalize(v.vertex.xyz);

                v2f o;
                o.pos = mul(UNITY_MATRIX_VP,float4(world,1));//v.vertex);
                //return o;


               
                o.uv = v.texcoord.xy;
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
              
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }

          Tags{ "Queue" = "Geometry"}
        Pass{
        CGPROGRAM
        #pragma vertex vert
            #pragma fragment frag

                            struct appdata
    {
        float4 vertex : POSITION;
        float3 normal : NORMAL;
    };

    struct v2f
    {
        float4 pos : POSITION;
        float3 nor : TEXCOORD0;
        float3 eye : TEXCOORD1;
    };
         
         v2f vert(appdata v)
            {
                appdata original = v;

                float3 world = mul( unity_ObjectToWorld, float4( v.vertex.xyz,1 ));
                float3 wNorm = mul( unity_ObjectToWorld, float4( v.normal,0 ));
                if( length( _Velocity ) > .01 ){
                    world += (-_Velocity * .04 + wNorm * dot( normalize(_Velocity) , wNorm ) * .1 + _Velocity * .03 * dot(normalize(_Velocity),wNorm )) * clamp( length(_Velocity) * 10, 0 ,1);
                }
                //v.vertex.xyz += _OutlineWidth * normalize(v.vertex.xyz);

                v2f o;

                o.nor = wNorm;
                o.eye = world - _WorldSpaceCameraPos.xyz;
                o.pos = mul(UNITY_MATRIX_VP,float4(world,1));//v.vertex);
                return o;

            }

            samplerCUBE _CubeMap;
            half4 frag(v2f v) : COLOR
            {
                float4 col = 1;

                float m = dot( normalize(v.eye) , - v.nor);
                float3 refl = reflect( v.eye , -v.nor );
                float3 tCol = texCUBE(_CubeMap, refl);
                col.xyz =tCol* 2 * tex2D(_GlobalColorMap,m * .4) * (1-m);// normalize(v.nor) * .5 + .5;
                return col;
            }

        ENDCG
       }

    }
    //Fallback "Diffuse"
}
