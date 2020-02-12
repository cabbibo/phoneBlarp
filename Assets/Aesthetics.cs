using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aesthetics : MonoBehaviour
{

    public int colorScheme;
    public int oColorScheme;
    public Texture2D[] colors;
    public float colorSchemeChangeTime;

    public Vector3 location;
    public Transform growthTransform;
    public Renderer growthRenderer;
    public float changeSpeed;
    public bool changing;
    public float changeLerpVal;

    public TouchBlarp game;
    public void SetNewColorScheme(){

      oColorScheme = colorScheme;
      if( oColorScheme == -1 ){ oColorScheme = 0; }
      colorSchemeChangeTime = Time.time;
      colorScheme ++;
      colorScheme %= colors.Length;
      changeLerpVal = 0;



      Shader.SetGlobalTexture("_GlobalColorMap", colors[colorScheme]);
      Shader.SetGlobalTexture("_OldGlobalColorMap", colors[oColorScheme]);
      Shader.SetGlobalFloat("_ColorMapLerpVal", changeLerpVal);
      location = game.blarp.transform.position;

      growthRenderer.sharedMaterial.SetVector( "_ChangeLocation" , game.colorChangeTarget.transform.position);
      growthRenderer.enabled = true;
      changing = true;

    }

    void Update(){
      if( changing == true ){
        float v = Time.time - colorSchemeChangeTime;
        v /= changeSpeed;
        changeLerpVal = v;
        Shader.SetGlobalFloat("_ColorMapLerpVal", v);
        if( v > 1 ){
          growthRenderer.enabled = false;
          changing = false;
        }
      }
    }
    public void Restart(){
      colorScheme = -1;
      SetNewColorScheme();
    }
}
