using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.iOS;
using IMMATERIA;


[ExecuteAlways]
public class TouchBlarp : Game
{

    public GameObject colorChangeTarget;
    public GameObject tailGrowTarget;
    public GameObject grassGrower;

    public TubeTransfer enemyHairBody;

    public Hair vectorHair;
    public Hair enemyHair;
    public Aesthetics aesthetics;
    public GameObject scene;

    public GameObject blarp;
    public GameObject touch;
    public GameObject target;
    public TargetInfo targetInfo;
    public Glitch glitch;
    public Breath breath;

    public int tailGrowthModulus;
    public int colorChangeModulus;




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

    public int tailSize;

    public Transform highScoreTransform;

    public float sharkStartMass;
    public float sharkMinMass;
    public int sharkMinMassScore;


    public float blarpStartMass;
    public float blarpMinMass;
    public int blarpMinMassScore;

    public float startTargetLength;
    public float minTargetLength;
    public int minTargetLengthScore;

     public float startTargetRespawnSpeed;
    public float minTargetRespawnSpeed;
    public int minTargetRespawnSpeedScore;



    public float targetLength(){
      float v1 = startTargetLength - minTargetLength;
      float v2 =  (float)score/(float) minTargetLengthScore;
      float fMass = startTargetLength - v1 * v2;
      fMass = Mathf.Clamp(fMass , minTargetLength, startTargetLength );
      return fMass;
    }

     public float targetRespawnSpeed(){
      float v1 = startTargetRespawnSpeed - minTargetRespawnSpeed;
      float v2 =  (float)score/(float) minTargetRespawnSpeedScore;
      float fMass = startTargetRespawnSpeed - v1 * v2;
      fMass = Mathf.Clamp(fMass , minTargetRespawnSpeed, startTargetRespawnSpeed );
      return fMass;
    }

    public float BlarpMass(){
      float v1 = blarpStartMass - blarpMinMass;
      float v2 =  (float)score/(float)blarpMinMassScore;
      float fMass = blarpStartMass - v1 * v2;
      fMass = Mathf.Clamp(fMass , blarpMinMass , blarpStartMass );
      return fMass;
    }

    public float SharkMass(){
      float v1 = sharkStartMass - sharkMinMass;
      float v2 =  (float)score/(float)sharkMinMassScore;
      float fMass = sharkStartMass - v1 * v2;
      fMass = Mathf.Clamp(fMass , sharkMinMass , sharkStartMass );
      return fMass;
    }


    public override void OnAwake()
    {



        blarpStartingPosition = blarp.transform.position;
        touchStartingPosition = touch.transform.position;
        //targetStartingPosition = target.transform.position;
        sharkStartingPosition = new Vector3( 100 , 0 , -100);//shark.transform.position;
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


    private int frame;
    void Start(){

     frame = 0;
    }


    
    void Update()
    {


      frame ++;
      if( frame ==1){
        DoStart();
        DoNewScore();
        DoRestart();
      }


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


            touchLR.SetPosition( 0 , blarp.transform.position + dif * .2f -Camera.main.transform.forward * upConnectionDist * 2);
            touchLR.SetPosition( 1 , blarp.transform.position + dif * .5f -Camera.main.transform.forward * upConnectionDist * 2);
            touchLR.SetWidth(.6f, 0);

            touchAudioSource.pitch  =  Mathf.Clamp( 4/(touch.transform.position - blarp.transform.position).magnitude,0 , 10);
            touchAudioSource.volume = Mathf.Lerp( touchAudioSource.volume , 1 , .5f );

          }else{ 

            if( breath.cooling == false ){ breath.breathing = true; }
            
            touch.transform.position = Vector3.up * 11;
          
            touchLR.SetPosition( 0 , Vector3.zero );
            touchLR.SetPosition( 1 , Vector3.zero );

            touchAudioSource.volume = Mathf.Lerp( touchAudioSource.volume , 0 , .1f );



          }


          // Pull towards blarp
          if( playing  && !inMenu ){




              sharkLR.SetPosition( 0 , shark.transform.position-Camera.main.transform.forward * upConnectionDist * .1f);
              sharkLR.SetPosition( 1 , blarp.transform.position-Camera.main.transform.forward * upConnectionDist * .1f);
              float dist = (shark.transform.position - blarp.transform.position).magnitude;
              sharkRB.AddForce(Vector3.Scale((shark.transform.position - blarp.transform.position),new Vector3(1,0,1)) * -.3f );
             
             
      
          }else{
            sharkLR.SetPosition( 0 , Vector3.zero);
            sharkLR.SetPosition( 1 , Vector3.zero);

          }


          if( inMenu ){

            //blarpRigidBody.velocity = Vector3.zero;
//            sharkRigidBody.velocity = Vector3.zero;

            //blarp.transform.position = blarpStartingPosition;
            shark.transform.position = sharkStartingPosition;
            sharkRB.velocity = Vector3.zero;
            touch.transform.position = touchStartingPosition;
            //target.transform.position = targetStartingPosition;

          }


     
    }



    //Mathf.Clamp( .3f - (float)score * .001f  , .05f , .3f);
    

    public override void DoRestart(){


      //blarpRigidBody.velocity = Vector3.zero;
      
        blarpRigidBody.mass = blarpStartMass;
        sharkRB.useGravity = false;
        enemyHairBody.showBody = false;

       // highScoreTransform.position = blarp.transform.position;

     // blarp.transform.position = blarpStartingPosition;
      touch.transform.position = touchStartingPosition;
      //target.transform.position = targetStartingPosition;
      targetInfo.Restart();

      targetInfo.spawnLength = startTargetLength;
      targetInfo.timeBetweenSpawns = startTargetRespawnSpeed;
      shark.transform.position = sharkStartingPosition;
      TriggerGlitch();
      blarpHitSource.Play();

      //blarpTrail.time = .3f;
      //sharkTrail.time = .3f;
      //score = 0;

      tailGrowTarget.GetComponent<TailGrowerChanger>().Despawn();
      colorChangeTarget.GetComponent<ColorSchemeChanger>().Despawn();

      UpdateTransformBuffer();
    }

   

    public override void DoStart(){

      targetInfo.spawnLength = targetLength();
      targetInfo.timeBetweenSpawns = targetRespawnSpeed();
      sharkRB.useGravity = true;
      startHitSource.Play();
      blarpRigidBody.mass = BlarpMass();//Mathf.Clamp(.2f,0.00001f,1);
      haptics.TriggerSuccess();
      MiniGlitch();

      SpawnShark();

      UpdateTransformBuffer();

      tailSize = 0;
      aesthetics.Restart();
     // aesthetics.SetNewColorScheme();
    
    }

    public TransformBuffer transformBuffer;
    public void UpdateTransformBuffer(){
      List<Transform> transforms = new List<Transform>();
      transforms.Add( blarp.transform );
      transforms.Add( shark.transform );

      transforms.Add( touch.transform );
      transforms.Add( target.transform );
      transforms.Add( colorChangeTarget.transform );
      transforms.Add( tailGrowTarget.transform );
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

    

      targetInfo.spawnLength = targetLength();
      targetInfo.timeBetweenSpawns = targetRespawnSpeed();
        vectorHair.length = (float)score/100;
        enemyHair.length = (float)score/100;
      
        blarpRigidBody.mass = BlarpMass();//.2f;//Mathf.Clamp(MASS(),0.00001f,1);
        currentMass= blarpRigidBody.mass;
          blarpTrail.time = .3f + (float)score / 20;
          sharkRB.mass = SharkMass();//Mathf.Clamp(MASS(),0.00001f,1);

        targetHitSource.Play();


        
        if( score >= 10 ){ enemyHairBody.showBody = true; }
          if( score % colorChangeModulus == 0 ){
            SpawnColorChange();
          }

          if( score % tailGrowthModulus == 0 ){
            SpawnTailChange();
          }
        

        UpdateScore();

        MiniGlitch();
    }


    public void SpawnColorChange(){
      colorChangeTarget.GetComponent<ColorSchemeChanger>().OnSpawn();
    }


    public void SpawnTailChange(){
      tailGrowTarget.GetComponent<TailGrowerChanger>().OnSpawn();
    }

 

    public void ColorChangeHit(){
      score += 5;
      UpdateScore();
      aesthetics.SetNewColorScheme();
    }
    public void UpdateScore(){

      scoreText.text = ""+score;
      Shader.SetGlobalInt("_Score" , score );

    }

    public void TailGrow(){
      tailSize ++;
      Shader.SetGlobalInt("_TailSize" , score );
      score += 3;
      UpdateScore();

    }

    public void SpawnShark(){
    
      shark.transform.position  = blarp.transform.position - blarpRigidBody.velocity.normalized * 3;// -Camera.main.transform.forward;
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