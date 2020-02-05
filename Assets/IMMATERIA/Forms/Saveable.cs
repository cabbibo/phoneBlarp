
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace IMMATERIA {
public class Saveable {


  public static List<string> names = new List<string>();
  
  
  public static string GetSafeName(){

    string fString = "entity"+ UnityEngine.Random.Range(0,10000000);


    Debug.Log("NAME: " + fString );
    foreach( string s in names ){
      if( fString  == s){
        fString = "entity"+ UnityEngine.Random.Range(0,10000000);//GetSafeName();
      }
    }

    names.Add( fString );
    return fString;

  }



  public static bool CheckIfAllNamesSafe(){

    bool allSafe = true;
    int i1 = 0;
    int i2 = 0;
    foreach( string s1 in names ){
    i1 ++;
    i2 = 0;
    foreach( string s2 in names ){
      i2++;
      if( s1 == s2 && i1 != i2 ){
        allSafe = false;
      }
    }
    }

    return allSafe;
  }

  public static void ClearNames(){
    names.Clear();
  }




  public static string GetFullName( string name ){
    return Application.persistentDataPath+"/DNA/"+name+".dna";
  }


  public static void DeleteAll(){

    CheckDNA();
    string path = Application.persistentDataPath + "/DNA";


    var hi = Directory.GetFiles(path);

     for (int i = 0; i < hi.Length; i++) {
         File.Delete(hi[i]);
     }
  }
  public static bool Check( string name ){

    if( name == null ){ Debug.Log("NO NAME"); }
    if( String.IsNullOrEmpty(name)){ Debug.Log("NO NAME111"); }
    return File.Exists(GetFullName(name));
  }

  public static void Delete(string name ){
    File.Delete( GetFullName(name));
  }




  public static void Save( Form form){

    CheckDNA();
    BinaryFormatter bf = new BinaryFormatter();
    FileStream stream = new FileStream(GetFullName(form.saveName),FileMode.Create);

    if( form.intBuffer ){
      int[] data = form.GetIntDNA();
      bf.Serialize(stream,data);
    }else{
      float[] data = form.GetDNA();
      bf.Serialize(stream,data);
    }

    stream.Close();
  }

  public static void CheckDNA(){
    if(!Directory.Exists(Application.persistentDataPath + "/DNA")){    
       //if it doesn't, create it
      Directory.CreateDirectory(Application.persistentDataPath + "/DNA");
    }
  }
  public static void Load(Form form){
    CheckDNA();
    if( File.Exists(GetFullName(form.saveName))){
      
      BinaryFormatter bf = new BinaryFormatter();
      FileStream stream = File.OpenRead(GetFullName(form.saveName));


      
      if( form.intBuffer ){
        int[] data = bf.Deserialize(stream) as int[];
         if( data == null ){
          form.DebugThis("YOUR  DATA IS NULL");
          form.saveName = GetSafeName();
          form.Embody();
          form.loadedFromFile = false;
          Saveable.Save(form);
        }else{
          if( data.Length != form.count * form.structSize ){
            form.DebugThis("YOUR INPUT DATA IS OFF");
            form.saveName = GetSafeName();
            form.Embody();
            form.loadedFromFile = false;
            Saveable.Save(form);

          }else{
            //form.DebugThis("loadedFromFileee");
            form.SetDNA(data);
          }
        }
      }else{
        if( stream.Length == 0 ){
          form.DebugThis("Stream is empty");
              form.saveName = GetSafeName();
              form.Embody();
              form.loadedFromFile = false;
              Saveable.Save(form);

        }else{
          float[] data = bf.Deserialize(stream) as float[];
          if( data == null ){
              form.DebugThis("YOUR  DATA IS NULL");
              form.saveName = GetSafeName();
              form.Embody();
              form.loadedFromFile = false;
              Saveable.Save(form);
            }else{
            if( data.Length != form.count * form.structSize ){
              form.DebugThis("YOUR INPUT DATA IS OFF");
              form.saveName = GetSafeName();
              form.Embody();
              form.loadedFromFile = false;
              Saveable.Save(form);

            }else{
              
             // form.DebugThis("loadedFromFileee");
              form.SetDNA(data);
            }
          }
        }
      }

      stream.Close();
    }else{
      Debug.Log("Why would you load something that doesn't exist?!??!?");
    }
  }




}
}