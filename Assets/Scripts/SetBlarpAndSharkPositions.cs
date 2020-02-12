using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetBlarpAndSharkPositions : MonoBehaviour
{

  public Transform blarp;
  public Transform shark;

  public Renderer render;

  void Start(){
    render = GetComponent<Renderer>();
  }
   
    // Update is called once per frame
    void Update()
    {
        render.sharedMaterial.SetVector("_BlarpPos", blarp.position);
        render.sharedMaterial.SetVector("_SharkPos", shark.position);
    }
}
