Shader "Unlit/wakk"
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
                UNITY_FOG_COORDS(1)
                float4 pos : SV_POSITION;
                float4 closest : TEXCOORD2;
                float3 world : TEXCOORD3;
            };

            sampler2D _MainTex;
            sampler2D _GlobalColorMap;
            float4 _MainTex_ST;

            uniform float4 _Hits[10];
            uniform int _Cooling;


            uniform float3 _BlarpPos;
            uniform float3 _SharkPos;


            v2f vert (appdata v)
            {
                v2f o;

                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                float3 world = mul( unity_ObjectToWorld, float4( v.vertex.xyz,1 ));
            
                float4 closest = 1000000;
                for( int i = 0; i <10; i++){
                    if( length( world - _Hits[i].xyz) < length(closest.xyz)){
                        closest = float4(world - _Hits[i].xyz , _Hits[i].w);
                    }
                }

                o.world = world;
                o.closest =  float4(world - _Hits[0].xyz , _Hits[0].w);// closest;
                o.pos = mul(UNITY_MATRIX_VP,float4(world,1));//v.vertex);
                return o;
            }

            fixed4 frag (v2f v) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, v.uv);

                col = 0;
                col = float4(1,0,0,1);
                col = float4(normalize(v.closest.xyz) ,1);//saturate( 10- length(v.closest) * 1 );
                col = 4*tex2D(_GlobalColorMap,length(v.closest.xyz) * .1 - _Time.y )  * saturate(1-(_Time.y-v.closest.w)) / length(v.closest.xyz);
                if(_Cooling == 1 ){
                    col = (sin( _Time.y  * 12 )+1) * float4(1,0,0,1);
                }

                float2 xz;
                float3 oCol;
                xz = abs(_BlarpPos.xz - v.world.xz);
                if( xz.y < .1 - v.uv.x * v.uv.x * .1 || xz.x < .1 - v.uv.y * v.uv.y * .1 ){ col = tex2D(_GlobalColorMap,min(xz.x,xz.y) * 1); }

                xz = abs(_SharkPos.xz - v.world.xz);
                if( xz.y < .1 - v.uv.x * v.uv.x * .1|| xz.x < .1 - v.uv.y * v.uv.y * .1){ col = tex2D(_GlobalColorMap,min(xz.x,xz.y) * 1 + .5); }

                return col;
            }
            ENDCG
        }
    }
}
