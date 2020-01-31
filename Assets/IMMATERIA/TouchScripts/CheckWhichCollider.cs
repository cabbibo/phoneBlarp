using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckWhichCollider : MonoBehaviour
{

    public TouchToRay touch;

    public void OnTap(){


        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(touch.RayOrigin,touch.RayDirection, out hit, Mathf.Infinity))
        {
            Debug.Log("Did Hit");
            Debug.Log( hit.collider.name);
        }
        else
        {
           Debug.Log("Did not Hit");
        }

    }
}
