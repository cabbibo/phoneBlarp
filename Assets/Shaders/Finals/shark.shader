Shader "Outlined/shark"
{
    Properties
    {
        _Color("Main Color", Color) = (0.5,0.5,0.5,1)
        _MainTex ("Texture", 2D) = "white" {}
        _OutlineColor ("Outline color", Color) = (0,0,0,1)
        _OutlineWidth ("Outlines width", Range (0.0, 2.0)) = 1.1
    }

    CGINCLUDE
    #include "UnityCG.cginc"

    #include "../Chunks/noise.cginc"

    struct appdata
    {
        float4 vertex : POSITION;
        float3 normal : NORMAL;
    };

    struct v2f
    {
        float4 pos : POSITION;
    };

    uniform float _OutlineWidth;
    uniform float4 _OutlineColor;
    uniform sampler2D _MainTex;
    uniform float4 _Color;
    uniform float3 _Velocity;

    float3 offset( float3 p , float3 n , float o ){
        return noise( p * 5 + _Time.y ) * normalize(n) * o;
    }

    ENDCG

    SubShader
    {
        

    

       Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" }

        Pass //Outline
        {
            ZWrite Off
            Cull Back
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            v2f vert(appdata v)
            {
                appdata original = v;

                 float3 world = mul( unity_ObjectToWorld, float4( v.vertex.xyz,1 ));
                float3 wNorm = mul( unity_ObjectToWorld, float4( v.normal,0 ));
            
                world += offset(world , wNorm , .1 + _OutlineWidth);
                //v.vertex.xyz += _OutlineWidth * normalize(v.vertex.xyz);

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

            Tags{ "Queue" = "Geometry"}
        Pass{
        CGPROGRAM
        #pragma vertex vert
            #pragma fragment frag
         
         v2f vert(appdata v)
            {
                appdata original = v;

                float3 world = mul( unity_ObjectToWorld, float4( v.vertex.xyz,1 ));
                float3 wNorm = mul( unity_ObjectToWorld, float4( v.normal,0 ));

                 world += offset(world , wNorm , .1);
              
                v2f o;
                o.pos = mul(UNITY_MATRIX_VP,float4(world,1));//v.vertex);
                return o;

            }

            half4 frag(v2f i) : COLOR
            {
                return 1;
            }

        ENDCG
       }

    }
    Fallback "Diffuse"
}
