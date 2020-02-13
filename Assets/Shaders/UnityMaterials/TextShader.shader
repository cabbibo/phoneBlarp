Shader "Unlit/textShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 world : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _GlobalColorMap;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.world = mul(unity_ObjectToWorld,v.vertex).xyz;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f v) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, v.uv);

                if( col.a < .1 ){
                    discard;
                }
                col = .1 * col.a + .9;
                if( col.a < .9 ){ col = 0; }
                col *= tex2D(_GlobalColorMap , v.world.x * .1 + v.world.z * .1 + _Time.y );
                return col;
            }
            ENDCG
        }
    }
}
