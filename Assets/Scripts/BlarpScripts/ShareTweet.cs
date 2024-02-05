using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShareTweet : MonoBehaviour
{

  public TouchBlarp game;
  public int shareID;

  void OnMouseDown()
  {
    if (shareID == 0)
    {
      game.ShareToTwitter();
    }
    else if (shareID == 1)
    {
      game.ShareToFacebook();
    }
    else
    {
      game.ShareToAppStore();
    }
  }
}
