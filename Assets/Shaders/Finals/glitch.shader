Shader "Finals/Glitch" {
 Properties {
 _MainTex ("Base (RGB)", 2D) = "white" {}
_GlitchSize("Size", float) = 0
_GlitchAmount("Amount", float) = 0
 }
 SubShader {
 Pass {
 CGPROGRAM
 #pragma vertex vert_img
 #pragma fragment frag
 
 #include "UnityCG.cginc"
 #include "../Chunks/noise.cginc"
 
 uniform sampler2D _MainTex;

 float _GlitchPower;
 float _GlitchAmount;
 float _GlitchSize;
 float _UpDown;
 float _SwipeVal;
 
 float4 frag(v2f_img i) : COLOR {



    float block_threshR = pow(frac(_Time.y * 1238.0453), 2.0) * 1;
    float block_threshG = pow(frac(_Time.y * 1538.0453), 2.0) * 1;
    float block_threshB = pow(frac(_Time.y * 1938.0453), 2.0) * 1;


    float bander = i.uv.y * 10 * _GlitchSize;
  

    float fSwipeVal = 1;
    if( _SwipeVal < 0 ){ fSwipeVal = 0; }

    float noiseR = abs(noise( float3( floor(hash( bander * .00001 + (fSwipeVal-.5)*_Time * .001) * 20 ) + floor(hash( bander * .000033 + (fSwipeVal-.5)*_Time.y * .00001) * 20 ), 1, frac((fSwipeVal-.5)*_Time.y * 20))));
    float noiseG = abs(noise( float3( floor(hash( bander * .000011 + (fSwipeVal-.5)*_Time * .001) * 20 ) + floor(hash( bander * .0000332 + (fSwipeVal-.5)*_Time.y * .00001) * 20 ), 1, frac((fSwipeVal-.5)*_Time.y * 20))));
    float noiseB = abs(noise( float3( floor(hash( bander * .000012 + (fSwipeVal-.5)*_Time * .001 ) * 20 ) + floor(hash( bander * .0000334 + (fSwipeVal-.5)*_Time.y * .00001) * 20 ), 1, frac((fSwipeVal-.5)*_Time.y * 20))));
   // if( sin(i.uv.y * 20.) < block_threshR && sin(i.uv.y * 10.) > block_threshR - _GlitchPower*2 ){ uvR = float2(uvR.x + .2*sin(floor(uvR.y * 10)) * _GlitchPower,uvR.y); }
   // if( sin(i.uv.y * 20.) < block_threshG && sin(i.uv.y * 10.) > block_threshG - _GlitchPower*2 ){ uvG = float2(uvG.x + .2*sin(floor(uvR.y * 10)) * _GlitchPower,uvG.y); }
   // if( sin(i.uv.y * 20.) < block_threshB && sin(i.uv.y * 10.) > block_threshB - _GlitchPower*2 ){ uvB = float2(uvB.x + .2*sin(floor(uvR.y * 10)) * _GlitchPower,uvB.y); }

   fSwipeVal = -fSwipeVal + 1;

    float3 fCol;// = float3( tex2D( _MainTex , uvR ).r , tex2D( _MainTex , uvG ).g , tex2D( _MainTex , uvB ).b );
    float fR = 2* (noiseR-.3) * (  -.5) *pow(1-_GlitchPower,3)* .4;
    float fG = 2* (noiseG-.3) * (  -.5) *pow(1-_GlitchPower,3)* .4;
    float fB = 2* (noiseB-.3) * (  -.5) *pow(1-_GlitchPower,3)* .4;
    float fColX = tex2D(_MainTex , i.uv + float2( fR  * _GlitchAmount , 0 ) ).x;
    float fColY = tex2D(_MainTex , i.uv + float2( fG  * _GlitchAmount , 0 ) ).y;
    float fColZ = tex2D(_MainTex , i.uv + float2( fB  * _GlitchAmount , 0 ) ).z;
    return float4(fColX, fColY, fColZ ,1);
 }
 ENDCG
 }
 }
}