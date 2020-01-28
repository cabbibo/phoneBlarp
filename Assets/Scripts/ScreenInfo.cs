using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenInfo : MonoBehaviour
{

    public float width;
    public float height;

    public void SetScreenSize(){
      Camera cam = Camera.main;
        height = 2f * cam.orthographicSize;
        width = height * cam.aspect;
    }

}
