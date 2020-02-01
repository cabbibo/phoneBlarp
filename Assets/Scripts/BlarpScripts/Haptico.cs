using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Haptico : MonoBehaviour
{

  public bool supported;
    // Start is called before the first frame update
    void Start()
    {

      supported = iOSHapticFeedback.Instance.IsSupported();
      if (supported){
          Debug.Log("iOS Haptic Feedback supported");
          iOSHapticFeedback.Instance.IsEnabled = true;
      }else{
          Debug.Log("Your device does not support iOS haptic feedback");
      }

//      print( (iOSHapticFeedback.iOSFeedbackType)0 );
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TriggerSelectionChange(){
      if( Application.isPlaying) iOSHapticFeedback.Instance.Trigger((iOSHapticFeedback.iOSFeedbackType)0);
    }
    
    public void TriggerImpactLight(){
       if( Application.isPlaying) iOSHapticFeedback.Instance.Trigger((iOSHapticFeedback.iOSFeedbackType)1);
    }
    public void TriggerImpactMedium(){
       if( Application.isPlaying) iOSHapticFeedback.Instance.Trigger((iOSHapticFeedback.iOSFeedbackType)2);
    }
     public void TriggerImpactHeavy(){
       if( Application.isPlaying) iOSHapticFeedback.Instance.Trigger((iOSHapticFeedback.iOSFeedbackType)3);
    }

    public void TriggerSuccess(){
       if( Application.isPlaying) iOSHapticFeedback.Instance.Trigger((iOSHapticFeedback.iOSFeedbackType)4);
    }

    public void TriggerWarning(){
       if( Application.isPlaying) iOSHapticFeedback.Instance.Trigger((iOSHapticFeedback.iOSFeedbackType)5);
    }

    public void TriggerFailuer(){
      iOSHapticFeedback.Instance.Trigger((iOSHapticFeedback.iOSFeedbackType)5);
    }

}