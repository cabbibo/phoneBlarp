using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowInfo : MonoBehaviour
{
  public GameObject info;
  public bool active;
  public Renderer image;

  public Texture activeImg;
  public Texture unactiveImg;


  void OnMouseDown()
  {
    if (active == false)
    {
      TurnOn();
    }
    else
    {
      TurnOff();
    }

  }

  public void TurnOn()
  {
    active = true;
    info.SetActive(true);
    image.sharedMaterial.SetTexture("_MainTex", activeImg);
  }
  public void TurnOff()
  {
    active = false;
    info.SetActive(false);

    image.sharedMaterial.SetTexture("_MainTex", unactiveImg);
  }
}
