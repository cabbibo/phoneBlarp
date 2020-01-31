 using UnityEngine;
 
 public class AudioSourceLoudnessTester : MonoBehaviour {
 
     public AudioSource audioSource;
     public float updateStep = 0.1f;
     public int sampleDataLength = 1024;
 
     private float currentUpdateTime = 0f;
 
     public float clipLoudness;
     public float currentLoudness;
     private float[] clipSampleData;
 
     // Use this for initialization
     void Awake () {
     
         if (!audioSource) {
             Debug.LogError(GetType() + ".Awake: there was no audioSource set.");
         }
         clipSampleData = new float[128];
 
     }
     
     // Update is called once per frame
     void Update () {
     
         currentUpdateTime += Time.deltaTime;
         if (currentUpdateTime >= updateStep) {
             currentUpdateTime = 0f;
             audioSource.clip.GetData(clipSampleData, audioSource.timeSamples); //I read 1024 samples, which is about 80 ms on a 44khz stereo clip, beginning at the current sample position of the clip.
             currentLoudness = 0f;
             
             foreach (var sample in clipSampleData) {
                 currentLoudness += Mathf.Abs(sample);
             }

             currentLoudness /= 128;// clipSampleData[0]; //= audioSource.clip.samples * audioSource.clip.channels; //clipLoudness is what you are looking for
         }
 
    currentLoudness *= audioSource.mute ? 0 : 1; 

      clipLoudness = Mathf.Lerp( clipLoudness , currentLoudness , .3f );
     }
 
 }