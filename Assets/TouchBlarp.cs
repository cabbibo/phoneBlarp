using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.iOS;

public class TouchBlarp : MonoBehaviour
{

    public Haptico haptics;

    public GameObject scene;

    public GameObject blarp;
    public GameObject touch;
    public GameObject target;
    public GameObject menu;
    public Glitch glitch;


    public GameObject wallPrefab;

    public GameObject sharkPrefab;
    public GameObject sharkTrailPrefab;

    public LineRenderer touchLR;

    public bool holding;

    public Collider collider;

    public int score;
    public TextMesh scoreText;
    public TextMesh highScoreText;
    public TextMesh lastScoreText;

    public bool playing;
    public bool inMenu;
    public bool touchDown;

    private Vector3 blarpStartingPosition;
    private Vector3 sharkStartingPosition;
    private Vector3 touchStartingPosition;
    private Vector3 targetStartingPosition;

    private Rigidbody blarpRigidBody;
   // private Rigidbody sharkRigidBody;


    public int highScore;
    public int hasReviewed;

    //private AudioSource sharkAudioSource;
    private AudioSource touchAudioSource;


    public AudioSource targetHitSource;
    public AudioSource blarpHitSource;
    public AudioSource startHitSource;

    public TrailRenderer blarpTrail;
    public TrailRenderer sharkTrail;


    public float screenWidth;
    public float screenHeight;

    public float upConnectionDist;

    public ParticleSystem targetHitParticles;
    public ParticleSystem sharkParticles;
    public ParticleSystem.EmissionModule sharkEmission;

    public float currentMass;

    public List<GameObject>sharks;
    public List<GameObject>sharkTrails;
    public List<LineRenderer>sharksLR;
    public List<TrailRenderer>sharksTR;
    public List<ParticleSystem>sharksPS;
    public List<AudioSource>sharksAS;
    public List<Rigidbody>sharksRB;

    public float spawnTime;

    void Awake()
    {

        Application.targetFrameRate = 60;


        blarpStartingPosition = blarp.transform.position;
        //sharkStartingPosition = shark.transform.position;
        touchStartingPosition = touch.transform.position;
        targetStartingPosition = target.transform.position;


        blarpRigidBody = blarp.GetComponent<Rigidbody>();
//        sharkRigidBody = shark.GetComponent<Rigidbody>();

        highScore =  PlayerPrefs.GetInt ("highScore");
        hasReviewed =  PlayerPrefs.GetInt ("hasReviewed");

//        sharkAudioSource = shark.GetComponent<AudioSource>();
        touchAudioSource = touch.GetComponent<AudioSource>();

        //blarpTrail  = blarp.GetComponent<TrailRenderer>();

         Camera cam = Camera.main;
        screenHeight = 2f * cam.orthographicSize;
        screenWidth = screenHeight * cam.aspect;

        MakeWalls();
        AnimationCurve curve = new AnimationCurve();

        curve.AddKey(0.0f, 1.0f);
        curve.AddKey(1.0f, 0.8f);


        blarpTrail.widthCurve = curve;
        blarpTrail.widthMultiplier = 5.5f;

         sharkTrail.widthCurve = curve;
        sharkTrail.widthMultiplier = 3.5f;
        //sharkEmission = sharkParticles.emission;


    }

    public void MakeWalls(){

      GameObject go;
        go = Instantiate( wallPrefab );
        go.transform.position = new Vector3( screenWidth /2 , 0 , 0 );
        go.transform.localScale = new Vector3( .1f  , 2 , screenHeight );
      
        wallMat = go.GetComponent<Renderer>().sharedMaterial;

        go = Instantiate( wallPrefab );
        go.transform.position = new Vector3( -screenWidth /2 , 0 , 0 );
        go.transform.localScale = new Vector3( .1f  , 2 , screenHeight );

        go = Instantiate( wallPrefab );
        go.transform.position = new Vector3( 0 ,0 , screenHeight/2 );
        go.transform.localScale = new Vector3( screenWidth , 2 , .1f );

        go = Instantiate( wallPrefab );
        go.transform.position = new Vector3( 0 , 0, -screenHeight/2 );
        go.transform.localScale = new Vector3( screenWidth , 2 , .1f  );

    }

    void Start(){
      Restart();
    }

    void Update()
    {

          if(Input.GetMouseButtonDown(0)){
            touchDown = true;
          }

          if (Input.GetMouseButton(0) && touchDown ){

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
            
            touch.transform.position = Vector3.one * 1000;
          
            touchLR.SetPosition( 0 , Vector3.zero );
            touchLR.SetPosition( 1 , Vector3.zero );

            touchAudioSource.volume = Mathf.Lerp( touchAudioSource.volume , 0 , .1f );



          }


          // Pull towards blarp
          if( playing  && !inMenu ){



            for( int i = 0; i< sharks.Count; i++ ){

              sharksLR[i].SetPosition( 0 , sharks[i].transform.position-Camera.main.transform.forward * upConnectionDist);
              sharksLR[i].SetPosition( 1 , blarp.transform.position-Camera.main.transform.forward * upConnectionDist);
              float dist = (sharks[i].transform.position - blarp.transform.position).magnitude;
              sharksRB[i].AddForce(Vector3.Scale((sharks[i].transform.position - blarp.transform.position),new Vector3(1,0,1)) * -.3f );
              sharksAS[i].pitch  =  Mathf.Clamp( 4/dist,0 , 10);
              sharksAS[i].volume = Mathf.Lerp( sharksAS[i].volume , 1 , .1f );
              //Time.timeScale = Mathf.Clamp( dist / 6 ,  0.3f,  1 );
              //print( dist);
              sharkEmission = sharksPS[i].emission;
              sharkEmission.rateOverTime = 1000/ (dist*dist*dist);

             
              
            }
          }else{/*
            sharkLR.SetPosition( 0 , Vector3.zero);
            sharkLR.SetPosition( 1 , Vector3.zero);

            sharkEmission = sharkParticles.emission;
            sharkEmission.rateOverTime = 0;
            sharkAudioSource.volume = Mathf.Lerp( sharkAudioSource.volume , 0 , .1f );*/
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
    public void Restart(){

      lastScoreText.text = ""+score;
      menu.SetActive(true);

      for( int i = 0; i < sharks.Count; i++ ){
        Destroy( sharks[i] );
        Destroy( sharkTrails[i] );
      }
      sharks.Clear();
      sharkTrails.Clear();
      sharksLR.Clear();
      sharksTR.Clear();
      sharksPS.Clear();
      sharksAS.Clear();
      sharksRB.Clear();

      if( score == highScore && score >= 20){
        AskForRating();
      }

     // score = 0;

      //scoreText.text = ""+score;


      highScoreText.text = ""+highScore;


      blarpRigidBody.velocity = Vector3.zero;
//      sharkRigidBody.velocity = Vector3.zero;
      
        blarpRigidBody.mass = 1;
//        sharkRigidBody.mass = 1;


      blarp.transform.position = blarpStartingPosition;
      //shark.transform.position = sharkStartingPosition;
      touch.transform.position = touchStartingPosition;
      target.transform.position = targetStartingPosition;

      playing = false;
      inMenu = true;
      touchDown = false;
      TriggerGlitch();
      blarpHitSource.Play();

      blarpTrail.time = .3f;
      sharkTrail.time = .3f;
      haptics.TriggerWarning();

    }

    public void Next(){

      if( !playing ){
        playing = true;
        menu.SetActive(false);//.gameObject.GetComponent<Renderer>().enabled = false;
        startHitSource.Play();
        score = 0;

        scoreText.text = ""+score;

      blarpRigidBody.mass = Mathf.Clamp(MASS(),0.00001f,1);

        haptics.TriggerSuccess();
        MiniGlitch();

        SpawnShark();
      }else{


        score ++;
        scoreText.text = ""+score;

        if( score % 5 == 0 ){
          //SpawnShark();
        }

        blarpRigidBody.mass = Mathf.Clamp(MASS(),0.00001f,1);


        currentMass= blarpRigidBody.mass;
          blarpTrail.time = .3f + (float)score / 20;
          for(int i = 0; i < sharks.Count;i++ ){
            sharksRB[i].mass = Mathf.Clamp(MASS(),0.00001f,1);
            sharksTR[i].time = .3f + (float)score / 30;
          }
        targetHitSource.Play();

        MiniGlitch();

        if (score > highScore){
          highScore = score;
      
          highScoreText.text = ""+highScore;
          PlayerPrefs.SetInt ("highScore", highScore);
        haptics.TriggerImpactHeavy();
        EmitParticles( 300 );
        }else{

        haptics.TriggerImpactLight();
        EmitParticles( 100 );
        }
      }

    }



    public void SpawnShark(){
      GameObject shark = Instantiate( sharkPrefab);
      GameObject sharkTrail = Instantiate( sharkTrailPrefab );

      shark.transform.position  = blarp.transform.position - blarp.GetComponent<Rigidbody>().velocity.normalized;

      sharkTrails.Add( sharkTrail );
      sharks.Add(shark);
      sharksLR.Add( sharkTrail.GetComponent<LineRenderer>() );
      sharksTR.Add( sharkTrail.GetComponent<TrailRenderer>() );
      sharksPS.Add( sharkTrail.GetComponent<ParticleSystem>() );
      sharksRB.Add( shark.GetComponent<Rigidbody>() );
      sharksAS.Add( shark.GetComponent<AudioSource>() );
      sharkTrail.GetComponent<CopyPosition>().target = shark.transform;
      sharkTrail.transform.parent = scene.transform;
      shark.transform.parent = scene.transform;
      sharkTrail.transform.localRotation = Quaternion.identity;
      spawnTime = Time.time;

      /*for( int i = 0; i < sharks.Count; i++ ){
        sharks[i].GetComponent<Collider>().enabled = false;
      }*/

      shark.GetComponent<Rigidbody>().mass = Mathf.Clamp(MASS(),0.00001f,1);
    }

    public void TriggerGlitch(){
    glitch.glitchPow = 0;
    glitch.glitchStartTime = Time.time;
    glitch.enabled = true;
    float p = Random.Range( 0, .99f);
    glitch.glitchLength = 1.5f + p;

    glitch.glitchSize = .1f;
    glitch.glitchAmount = 3.5f;

  }


  public void MiniGlitch(){
    glitch.glitchPow = 0;
    glitch.glitchStartTime = Time.time;
    glitch.glitchSize = .2f + (float)score/40;
    glitch.glitchAmount = .1f+ (float)score/300;
    glitch.enabled = true;
    float p = Random.Range( 0, .99f);
    glitch.glitchLength = .1f + p * .3f;
  }

  public void EmitParticles( int count ){
    targetHitParticles.Emit( count);
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


public Vector4[] hits;
public int currHit;
public Material wallMat;


public void SetWallCollision(Vector3 location){

  print( location );
  currHit += 1;
  currHit %= hits.Length;
  hits[0] = new Vector4( location.x , location.y , location.z , Time.time );

  wallMat.SetVectorArray( "_Hits" , hits );

  print("hi");
  



}


}