using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.iOS;

public class Game : MonoBehaviour
{

    public ScreenInfo screen;
    public Walls walls;
    

    public int score;
    public TextMesh scoreText;
    public TextMesh highScoreText;
    public TextMesh lastScoreText;

    public bool playing;
    public bool inMenu;
    public bool touchDown;

    public Transform socialButtons;
    public Transform title;

    public GameObject menu;


    public int highScore;
    public int hasReviewed;

    public Haptico haptics;


    public void _OnAwake(){
  
      walls.DestroyWalls();
    
      highScore =  PlayerPrefs.GetInt ("highScore");
      hasReviewed =  PlayerPrefs.GetInt ("hasReviewed");
      Application.targetFrameRate = 60;
      screen.SetScreenSize();
      walls.screen = screen;
      walls.MakeWalls();
      SetUpMenu();
      OnAwake();

    }

    public virtual void OnAwake(){

    }

    public virtual void Next(){

      if( !playing ){
        playing = true;
        menu.SetActive(false);//.gameObject.GetComponent<Renderer>().enabled = false;
        
        scoreText.gameObject.SetActive(true);
        score = 0;
        scoreText.text = ""+score;

        DoStart();

        score ++;
        scoreText.text = ""+score;

        if (score > highScore){
          highScore = score;
      
          highScoreText.text = "high : "+highScore;
          PlayerPrefs.SetInt ("highScore", highScore);
      
        DoNewHighScore();
        }else{

      
        DoNewScore();
        }

        DoNext();
      }else{


        score ++;
        scoreText.text = ""+score;

        if (score > highScore){
          highScore = score;
      
          highScoreText.text = ""+highScore;
          PlayerPrefs.SetInt ("highScore", highScore);
      
        DoNewHighScore();
        }else{

      
        DoNewScore();
        }

        DoNext();
      }
    }

    public virtual void DoStart(){}
    public virtual void DoNewScore(){}
    public virtual void DoNewHighScore(){}
    public virtual void DoNext(){}

    public virtual void Restart(){

      scoreText.gameObject.SetActive(false);//.enabled = false;
      lastScoreText.text = "last : "+score;
      menu.SetActive(true);

      if( score == highScore && score >= 20){
        AskForRating();
      }

      highScoreText.text = "high : "+highScore;

      playing = false;
      inMenu = true;
      touchDown = false;
      
      haptics.TriggerWarning();
      DoRestart();




    }

    public void SetUpMenu(){
      scoreText.transform.position = new Vector3( -screen.width * .4f ,  1 , screen.height * .45f );
      socialButtons.position = new Vector3( screen.width * .4f ,  1 , -screen.height * .45f );
      title.position = new Vector3( -screen.width * .4f , 1, screen.height * .45f  );
    }
    public virtual void DoRestart(){}

    // Start is called before the first frame update
    void Awake()
    {
        _OnAwake(); 
    }

    void Start(){
      Restart();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AskForRating(){

  if( PlayerPrefs.GetInt( "hasReviewed" ) != 1){
    PlayerPrefs.SetInt( "hasReviewed", 1);
    Device.RequestStoreReview();
  }
}


}
