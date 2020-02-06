using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aesthetics : MonoBehaviour
{

    public int colorScheme;
    public Texture2D[] colors;



    public void SetNewColorScheme(){

      colorScheme ++;
      colorScheme %= colors.Length;
      Shader.SetGlobalTexture("_GlobalColorMap", colors[colorScheme]);

    }

    public void Restart(){
      colorScheme = -1;
      SetNewColorScheme();
    }
}
