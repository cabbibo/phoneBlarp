using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetVelocity : MonoBehaviour
{

  public Transform other;
    private Material mat;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    { 
      mat = GetComponent<MeshRenderer>().material;
      if( other == null ){ other = transform; }
      rb = other.GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {
      mat.SetVector("_Velocity", rb.velocity);
        
    }
}
