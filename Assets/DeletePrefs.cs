using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
 
public class DeletePrefs : MonoBehaviour
{
    public void Delete()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}