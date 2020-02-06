using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CopyPosition : MonoBehaviour
{
   public Transform target;
   public float forwardOffset;

    // Update is called once per frame
    void LateUpdate()
    {
      transform.position = target.position + Camera.main.transform.forward * forwardOffset;;
    }
}
