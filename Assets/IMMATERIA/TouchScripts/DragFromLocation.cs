using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragFromLocation : MonoBehaviour
{

    public Transform targetLocation;

    public Vector3 force;
    public Vector3 vel;

    public Rigidbody rb;
    public void Start(){
      rb = GetComponent<Rigidbody>();
    }

    public void FixedUpdate(){
      force = targetLocation.position - transform.position;
      rb.AddForce( force * 1.1f );
      /*force = targetLocation.position - transform.position;
      vel += force * .01f;

      transform.position += vel;
      vel *= .95f;*/

    }
}
