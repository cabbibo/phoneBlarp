Shader "Outlined/target"
{
    Properties
    {
        _Color("Main Color", Color) = (0.5,0.5,0.5,1)
        _MainTex ("Texture", 2D) = "white" {}
        _OutlineColor ("Outline color", Color) = (0,0,0,1)
        _OutlineWidth ("Outlines width", Range (0.0, 2.0)) = 1.1
    }





    SubShader
    {
        

    

       Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" }

        Pass //Outline
        {
            Cull Back
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag



    #include "UnityCG.cginc"

    struct appdata
    {
        float4 vertex : POSITION;
        float4 uv : TEXCOORD0;
        float3 normal : NORMAL;
    };

    struct v2f
    {
        float4 pos : POSITION;
        float4 uv : TEXCOORD0;
    };

    uniform float _OutlineWidth;
    uniform float4 _OutlineColor;
    uniform sampler2D _MainTex;
    uniform sampler2D _GlobalColorMap;
    uniform float4 _Color;
    uniform float3 _Velocity;

            v2f vert(appdata v)
            {
                appdata original = v;

                v.vertex.z = 0;
          
                
                //v.vertex.xyz += _OutlineWidth * normalize(v.vertex.xyz);



                v2f o;
               o.pos = UnityObjectToClipPos(v.vertex );
                o.uv = v.uv;
                return o;

            }

            half4 frag(v2f i) : COLOR
            {

                float l = length(i.uv.xy - .5);
                float2 dif = i.uv.xy - .5;
                float a = atan2(dif.y , dif.x );
                
                float4 col = 0;
                if( l > .5 ){discard;}
                if( l < .4 && l > .33 ){discard;}
                if( sin(a * 8 -4* _Time.y) < 0 && l > .4 ){discard;}
                if( l > .25 ){ col = 0; }
                //if( l < .25){ discard; }
                //sin(a * 10 + _Time.y * 10 );///3.14;
                float4 s = tex2D( _GlobalColorMap,_Time.y * .3+ a/6.28);

                if( l > .4 ){ col = s; }
                return col;
            }

            ENDCG
        }

   

              // shadow caster rendering pass, implemented manually
        // using macros from UnityCG.cginc
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

            struct v2f { 
                V2F_SHADOW_CASTER;
                float2 uv: TEXCOORD1;
            };

            v2f vert(appdata_base v)
            {
                v2f o;
                TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
                o.uv = v.texcoord;
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {

                
                float l = length(i.uv.xy - .5);
                float2 dif = i.uv.xy - .5;
                float a = atan2(dif.y , dif.x );
                
                float4 col = 1;
                if( l > .5 ){discard;}
                if( l < .4 && l > .33 ){discard;}
                if( sin(a * 8 -4* _Time.y) < 0 && l > .4 ){discard;}
                if( l < .25){ discard; }
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }


    }
    Fallback "Diffuse"
}
