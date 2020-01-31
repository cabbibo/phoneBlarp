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

    public GameObject menu;


    public int highScore;
    public int hasReviewed;

    public Haptico haptics;


    public void _OnAwake(){
 
      highScore =  PlayerPrefs.GetInt ("highScore");
      hasReviewed =  PlayerPrefs.GetInt ("hasReviewed");
      Application.targetFrameRate = 60;
      screen.SetScreenSize();
      walls.screen = screen;
      walls.MakeWalls();
      OnAwake();

    }

    public virtual void OnAwake(){

    }

    public virtual void Next(){

      if( !playing ){
        playing = true;
        menu.SetActive(false);//.gameObject.GetComponent<Renderer>().enabled = false;
        
        score = 0;
        scoreText.text = ""+score;

        DoStart();
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


      lastScoreText.text = ""+score;
      menu.SetActive(true);

      if( score == highScore && score >= 20){
        AskForRating();
      }

      highScoreText.text = ""+highScore;

      playing = false;
      inMenu = true;
      touchDown = false;
      
      haptics.TriggerWarning();
      DoRestart();


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
