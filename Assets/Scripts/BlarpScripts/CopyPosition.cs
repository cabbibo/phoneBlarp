using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyPosition : MonoBehaviour
{
   public Transform target;
   public float forwardOffset;

    // Update is called once per frame
    void Update()
    {
      transform.position = target.position + Camera.main.transform.forward * forwardOffset;;
    }
}
