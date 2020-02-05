using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.iOS;
using IMMATERIA;


[ExecuteAlways]
public class TouchBlarp : Game
{

    public GameObject scene;

    public GameObject blarp;
    public GameObject touch;
    public GameObject target;
    public Glitch glitch;
    public Breath breath;



    public LineRenderer touchLR;

    public bool holding;

    public Collider collider;


    private Vector3 blarpStartingPosition;
    private Vector3 sharkStartingPosition;
    private Vector3 touchStartingPosition;
    private Vector3 targetStartingPosition;

    private Rigidbody blarpRigidBody;
   // private Rigidbody sharkRigidBody;



    //private AudioSource sharkAudioSource;
    private AudioSource touchAudioSource;


    public PlayRandomFromArray targetHitSource;
    public AudioSource blarpHitSource;
    public AudioSource startHitSource;

    public TrailRenderer blarpTrail;
    public TrailRenderer sharkTrail;


    public float upConnectionDist;

    public ParticleSystem targetHitParticles;
    public ParticleSystem sharkParticles;
    public ParticleSystem.EmissionModule sharkEmission;

    public float currentMass;

    public GameObject shark;
    public LineRenderer sharkLR;
    public Rigidbody sharkRB;

    public float spawnTime;

    public override void OnAwake()
    {



        blarpStartingPosition = blarp.transform.position;
        touchStartingPosition = touch.transform.position;
        targetStartingPosition = target.transform.position;
        blarpRigidBody = blarp.GetComponent<Rigidbody>();

        highScore =  PlayerPrefs.GetInt ("highScore");
        hasReviewed =  PlayerPrefs.GetInt ("hasReviewed");

        touchAudioSource = touch.GetComponent<AudioSource>();

        AnimationCurve curve = new AnimationCurve();

        curve.AddKey(0.0f, 1.0f);
        curve.AddKey(1.0f, 0.8f);


        //blarpTrail.widthCurve = curve;
        //blarpTrail.widthMultiplier = 5.5f;

        //sharkTrail.widthCurve = curve;
        //sharkTrail.widthMultiplier = 3.5f;


    }


    
    void Update()
    {

          if(Input.GetMouseButtonDown(0)){
            if( !breath.cooling ) touchDown = true;
          }

          if (Input.GetMouseButton(0) && touchDown ){
            
            breath.breathing = false;

            if( inMenu == true ){ inMenu = false; }

              Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
              RaycastHit hit;

              if (collider.Raycast(ray, out hit, 100.0f)){

                touch.transform.position = hit.point;

               
                blarpRigidBody.AddForce(Vector3.Scale((hit.point - blarp.transform.position),new Vector3(1,0,1)) * 1 );
              


              }



            Vector3 dif = touch.transform.position - blarp.transform.position;


            touchLR.SetPosition( 0 , blarp.transform.position + dif * .2f -Camera.main.transform.forward * upConnectionDist);
            touchLR.SetPosition( 1 , blarp.transform.position + dif * .5f -Camera.main.transform.forward * upConnectionDist);
            touchLR.SetWidth(.6f, 0);

            touchAudioSource.pitch  =  Mathf.Clamp( 4/(touch.transform.position - blarp.transform.position).magnitude,0 , 10);
            touchAudioSource.volume = Mathf.Lerp( touchAudioSource.volume , 1 , .5f );

          }else{ 

            if( breath.cooling == false ){ breath.breathing = true; }
            
            touch.transform.position = Vector3.one * 1000;
          
            touchLR.SetPosition( 0 , Vector3.zero );
            touchLR.SetPosition( 1 , Vector3.zero );

            touchAudioSource.volume = Mathf.Lerp( touchAudioSource.volume , 0 , .1f );



          }


          // Pull towards blarp
          if( playing  && !inMenu ){




              sharkLR.SetPosition( 0 , shark.transform.position-Camera.main.transform.forward * upConnectionDist);
              sharkLR.SetPosition( 1 , blarp.transform.position-Camera.main.transform.forward * upConnectionDist);
              float dist = (shark.transform.position - blarp.transform.position).magnitude;
              sharkRB.AddForce(Vector3.Scale((shark.transform.position - blarp.transform.position),new Vector3(1,0,1)) * -.3f );
             
             
      
          }else{
            sharkLR.SetPosition( 0 , Vector3.zero);
            sharkLR.SetPosition( 1 , Vector3.zero);

          }


          if( inMenu ){

            blarpRigidBody.velocity = Vector3.zero;
//            sharkRigidBody.velocity = Vector3.zero;

            blarp.transform.position = blarpStartingPosition;
            //shark.transform.position = sharkStartingPosition;
            touch.transform.position = touchStartingPosition;
            target.transform.position = targetStartingPosition;

          }


     if( glitch.glitchPow > 1 ){
          if(glitch.enabled == true ){
            glitch.enabled = false;

          }
        }

    }

    public float MASS(){
       return Mathf.Clamp( .3f - (float)score * .006f  , .05f , .3f);
    }



    public override void DoRestart(){


      blarpRigidBody.velocity = Vector3.zero;
      
        blarpRigidBody.mass = 1;


      blarp.transform.position = blarpStartingPosition;
      touch.transform.position = touchStartingPosition;
      target.transform.position = targetStartingPosition;
      shark.transform.position = Vector3.one * 1000;
      TriggerGlitch();
      blarpHitSource.Play();

      //blarpTrail.time = .3f;
      //sharkTrail.time = .3f;
      score = 0;

      UpdateTransformBuffer();
    }

   

    public override void DoStart(){
    
      startHitSource.Play();
      blarpRigidBody.mass = Mathf.Clamp(MASS(),0.00001f,1);
      haptics.TriggerSuccess();
      MiniGlitch();

      SpawnShark();

      UpdateTransformBuffer();
    }

    public TransformBuffer transformBuffer;
    public void UpdateTransformBuffer(){
      List<Transform> transforms = new List<Transform>();
      transforms.Add( blarp.transform );
      transforms.Add( shark.transform );

      transforms.Add( touch.transform );
      transforms.Add( target.transform );
      transformBuffer.Remake( transforms);
    }

    public override void DoNewHighScore(){
      haptics.TriggerImpactHeavy();
      EmitParticles( 300 );
    }

    public override void DoNewScore(){
      haptics.TriggerImpactLight();
      EmitParticles( 100 );
    }

    public override void DoNext(){
 
        blarpRigidBody.mass = Mathf.Clamp(MASS(),0.00001f,1);
        currentMass= blarpRigidBody.mass;
          blarpTrail.time = .3f + (float)score / 20;
          sharkRB.mass = Mathf.Clamp(MASS(),0.00001f,1);
        targetHitSource.Play();

        MiniGlitch();
    }

    public void SpawnShark(){
    
      shark.transform.position  = blarp.transform.position - Vector3.forward * 3 -Camera.main.transform.forward;
      sharkRB.velocity = Vector3.zero;

    }

    public void TriggerGlitch(){
    /*glitch.glitchPow = 0;
    glitch.glitchStartTime = Time.time;
    glitch.enabled = true;
    float p = Random.Range( 0, .99f);
    glitch.glitchLength = 1.5f + p;

    glitch.glitchSize = .1f;
    glitch.glitchAmount = 3.5f;*/

  }


  public void MiniGlitch(){
   /* glitch.glitchPow = 0;
    glitch.glitchStartTime = Time.time;
    glitch.glitchSize = .2f + (float)score/40;
    glitch.glitchAmount = .1f+ (float)score/300;
    glitch.enabled = true;
    float p = Random.Range( 0, .99f);
    glitch.glitchLength = .1f + p * .3f;*/
  }

  public void EmitParticles( int count ){
    //targetHitParticles.Emit( count);
  }


  private const string TWITTER_ADDRESS = "http://twitter.com/intent/tweet";
private const string TWEET_LANGUAGE = "en";
public static string descriptionParam;
private string appStoreLink = "http://www.YOUROWNAPPLINK.com";

public void ShareToTwitter()
{

    string nameParameter = "YOUR AWESOME GAME MESSAGE!";//this is limited in text length 
    Application.OpenURL(TWITTER_ADDRESS +
       "?text=" + WWW.EscapeURL(nameParameter + "\n" + descriptionParam + "\n" + "Get the Game:\n" + appStoreLink));
}

public void ShareToFacebook(){
  Application.OpenURL("https://www.facebook.com/sharer/sharer.php?u=https%3A%2F%2Fcabbi.bo%2F");
}

public void AskForRating(){

  if( PlayerPrefs.GetInt( "hasReviewed" ) != 1){
    PlayerPrefs.SetInt( "hasReviewed", 1);
    Device.RequestStoreReview();
  }
}


public void ShareToAppStore(){
  Application.OpenURL("https://apps.apple.com/us/developer/prism-simulations-llc/id1269904597");
}





}