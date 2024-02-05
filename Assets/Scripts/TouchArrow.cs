using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using UnityEngine.iOS;
using IMMATERIA;

public class TouchArrow : Game
{
  public Transform touchRep;
  public Collider collider;
  public Transform chaserRep;
  public Material touchMat;

  public GameObject EnemyPrefab;


  public GameObject lawn;
  public GameObject river;


  public Vector3 touchStart;
  public Vector3 touchCurrent;
  public Vector3 touchRelease;

  public Vector3 cPosition;
  public Vector3 cVelocity;
  public Vector3 cForce;

  public GameObject arrowPrefab;
  public GameObject enemyPrefab;
  public GameObject target;


  public LineRenderer lr;

  public bool holding;

  public List<GameObject> arrows;
  public List<GameObject> enemies;
  public float lastEnemyTime;

  public float lawnPosition;
  public float lawnSize;
  public float riverPosition;
  public float riverSize;

  public Collider lawnCollider;

  public TransformBuffer transformBuffer;

  public override void OnAwake()
  {

    touchMat = touchRep.GetComponent<Renderer>().material;

    lawn.transform.position = new Vector3(0, 2, -screen.height * .3f);
    lawn.transform.localScale = new Vector3(screen.width, screen.height * .2f, 1);

    river.transform.position = new Vector3(0, 3f, -screen.height * .3f);
    river.transform.localScale = new Vector3(screen.width, screen.height * .1f, 1);

    lawnCollider = lawn.GetComponent<Collider>();
  }

  private Vector3 force;
  private GameObject currentArrow;

  public float riverPos;

  void Update()
  {

    riverPos = screen.height * .5f - (1 - lawnSize) * screen.height;

    lawn.transform.position = new Vector3(0, .1f, riverPos - screen.height * lawnSize * .5f);
    lawn.transform.localScale = new Vector3(screen.width, screen.height * lawnSize, 1);

    river.transform.position = new Vector3(0, 3, riverPos);
    river.transform.localScale = new Vector3(screen.width, screen.height * riverSize, 1);

    Shader.SetGlobalFloat("_RiverPos", riverPos);

    force = Vector3.zero;
    if (Input.GetMouseButtonDown(0))
    {


      //var ray = Camera.main.ScreenPointToRay (Input.mousePosition);
      //touchRep.position = ray.origin + ray.direction  * 2;

      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      RaycastHit hit;

      if (lawnCollider.Raycast(ray, out hit, 100.0f))
      {
        SpawnArrow(hit.point);

        holding = true;
      }

    }

    if (Input.GetMouseButton(0))
    {

      //var ray = Camera.main.ScreenPointToRay (Input.mousePosition);
      //touchRep.position = ray.origin + ray.direction  * 2;

      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      RaycastHit hit;

      if (lawnCollider.Raycast(ray, out hit, 100.0f))
      {

        if (holding) UpdateArrowDirection(hit.point);


      }

    }
    else
    {


      if (currentArrow != null)
      {
        if (holding == true)
        {

          currentArrow.GetComponent<Rigidbody>().AddForce(noY((touchStart - chaserRep.position) * 100));
          chaserRep.position = currentArrow.transform.position;

        }

        if ((currentArrow.transform.position - touchStart).magnitude < .7f)
        {
          ReleaseArrow();
        }
      }

    }


    lr.SetPosition(0, touchRep.position - Camera.main.transform.forward * .2f);
    lr.SetPosition(1, chaserRep.position - Camera.main.transform.forward * .2f);


    UpdateArrows();


    if (playing)
    {
      UpdateEnemies();
    }

  }

  public Vector3 noY(Vector3 v)
  {
    return Vector3.Scale(v, -Vector3.left + Vector3.forward);
  }

  public void SpawnArrow(Vector3 location)
  {
    GameObject go = Instantiate(arrowPrefab);
    arrows.Add(go);
    currentArrow = go;
    go.GetComponent<ArrowInfo>().time = 1;

    currentArrow.GetComponent<Collider>().enabled = false;

    touchRep.position = location;
    touchStart = location;
    touchMat.color = new Color(1, 0, 0, 1);

    RemakeTransformBuffer();

  }

  public void UpdateArrows()
  {
    int id = 0;
    int removeID = -1;
    foreach (GameObject arrow in arrows)
    {

      if (arrow != null)
      {

        arrow.GetComponent<ArrowInfo>().time -= .003f;
        if (arrow.GetComponent<ArrowInfo>().time < 0) { removeID = id; }
        arrow.transform.localScale = Vector3.one * arrow.GetComponent<ArrowInfo>().time;
        //arrow.transform.position.y = arrow.transform.localScale * 2;
        id++;

      }

    }

    if (holding) currentArrow.GetComponent<ArrowInfo>().time = 1;
    if (removeID >= 0)
    {
      GameObject a = arrows[removeID];
      arrows.RemoveAt(removeID);
      DestroyImmediate(a);
    }


  }

  public void UpdateArrowDirection(Vector3 location)
  {
    chaserRep.position = location;
    touchCurrent = location;
    currentArrow.transform.position = location;
  }

  public void ReleaseArrow()
  {
    if (currentArrow) currentArrow.GetComponent<Collider>().enabled = true;
    holding = false;
    chaserRep.position = new Vector3(10000, 0, 0);
    touchRep.position = new Vector3(10000, 0, 0);
  }

  public void SpawnEnemy()
  {
    GameObject go = Instantiate(enemyPrefab);
    enemies.Add(go);
    go.transform.position = new Vector3(screen.width * Random.Range(-.5f, .5f), .5f, screen.height * .45f);
    lastEnemyTime = Time.time;

    RemakeTransformBuffer();
  }

  public void UpdateEnemies()
  {

    if (Time.time - lastEnemyTime > 3 / (1 + .03f * (float)score))
    {
      SpawnEnemy();
    }

    foreach (GameObject enemy in enemies)
    {
      enemy.GetComponent<Rigidbody>().AddForce(Vector3.forward * -3f);
    }

  }

  public void DestroyEnemy(GameObject g)
  {

    print(g);
    enemies.Remove(g);
    Destroy(g);

    RemakeTransformBuffer();
  }

  public void DestroyArrow(GameObject g)
  {

    print(g);
    arrows.Remove(g);
    Destroy(g);

    RemakeTransformBuffer();
  }



  public override void DoRestart()
  {
    ReleaseArrow();
    for (int i = 0; i < arrows.Count; i++)
    {
      Destroy(arrows[i]);
    }
    arrows.Clear();
    for (int i = 0; i < enemies.Count; i++)
    {
      Destroy(enemies[i]);
    }
    enemies.Clear();

    GameObject go = Instantiate(enemyPrefab);
    enemies.Add(go);
    go.transform.position = new Vector3(0, 0, screen.height * .1f);
    lastEnemyTime = Time.time + 10000;
    RemakeTransformBuffer();

  }

  public override void DoStart()
  {
    lastEnemyTime = Time.time;
  }

  public void RemakeTransformBuffer()
  {

    List<Transform> transforms = new List<Transform>();

    transforms.Add(touchRep);
    transforms.Add(chaserRep);

    for (int i = 0; i < arrows.Count; i++)
    {
      transforms.Add(arrows[i].transform);
    }

    for (int i = 0; i < enemies.Count; i++)
    {
      transforms.Add(enemies[i].transform);
    }

    transformBuffer.Remake(transforms);
  }


}