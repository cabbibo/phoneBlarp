using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class EditModeOsscilate : MonoBehaviour
{

  public float size;
  public float speed;

    // Update is called once per frame
    void Update()
    {

      if( Application.isPlaying == false ){

        float x = Mathf.Cos( Time.time * speed ) * size;
        float y = Mathf.Sin( Time.time * speed ) * size;
        transform.position = new Vector3(x, 2 , y);
        
    }
      }
}
