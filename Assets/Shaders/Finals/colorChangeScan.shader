Shader "Outlined/colorChangeScan"
{
    Properties
    {
        _Color("Main Color", Color) = (0.5,0.5,0.5,1)
        _ColorMap ("Texture", 2D) = "white" {}
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
        float3 world : TEXCOORD1;
        float  dif : TEXCOORD2;
    };

    uniform sampler2D _GlobalColorMap;
    uniform sampler2D _OldGlobalColorMap;
    uniform float _ColorMapLerpVal;
    uniform float3 _ChangeLocation;

            v2f vert(appdata v)
            {
                appdata original = v;

                v.vertex.z = 0;
          
                
                //v.vertex.xyz += _OutlineWidth * normalize(v.vertex.xyz);



                v2f o;

                o.world = mul( unity_ObjectToWorld, float4( v.vertex.xyz,1 ));
                //o.dif = length( o.world.xz - _ChangeLocation.xz) ;
               o.pos = UnityObjectToClipPos(v.vertex );
                o.uv = v.uv;
                return o;

            }

            #include "../Chunks/noise.cginc"

            half4 frag(v2f v) : COLOR
            {
float4 col = 0;
                /*float l = length(i.uv.xy - .5);
                float2 dif = i.uv.xy - .5;
                float a = atan2(dif.y , dif.x );

                l += noise( float3( dif.x , dif.y , _Time.y ) * 20 ) * .1;
                
                
                if( l > .5 ){discard;}
                //if( sin(a * 8 -4* _Time.y) < 0 && l > .4 ){discard;}
                if( l > .25 ){ col = 0; }
                //if( l < .25){ discard; }
                //sin(a * 10 + _Time.y * 10 );///3.14;
                float4 s = tex2D( _ColorMap,_Time.y * .3+ a/6.28);

                s =  tex2D( _ColorMap,_Time.y * .3+ l);
                col = s;
                if( l > .4 ){ col = s; }*/
                float d = length( v.world.xz - _ChangeLocation.xz);

                float n = noise( v.world.xyz * 3 );

                float newVal = (d - _ColorMapLerpVal*_ColorMapLerpVal*50)* 1.6+ n * .2;

                if(newVal< 0 ){ discard; }
                if(newVal> 1){ discard; }
                col = tex2D( _GlobalColorMap , newVal + _ColorMapLerpVal);
                //col = d * .1;
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