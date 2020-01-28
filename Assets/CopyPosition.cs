using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyPosition : MonoBehaviour
{
   public Transform target;

    // Update is called once per frame
    void Update()
    {
      transform.position = target.position + Camera.main.transform.forward * .3f;;
    }
}
