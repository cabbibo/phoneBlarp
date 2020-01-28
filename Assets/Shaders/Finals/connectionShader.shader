Shader "Unlit/connectionShader"
{
    Properties
    {
        _Size ("size", float) = 20
        _Cutoff ("Cutoff", float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "DisableBatching" = "True"}
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
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            float _Size;
            float _Cutoff;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture

                if( sin( i.uv.x * _Size) < _Cutoff){
                    discard;
                }
                fixed4 col = 0;//tex2D(_MainTex, i.uv);
                return col;
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
            #include "UnityCG.cginc"

            float _Size;
            float _Cutoff;
            struct v2f { 
                V2F_SHADOW_CASTER;

                float2 uv : TEXCOORD0;
            };

            v2f vert(appdata_base v)
            {
                v2f o;
                o.uv = v.texcoord.xy;
                TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                if( sin( i.uv.x * _Size) < _Cutoff){
                    discard;
                }
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
}
