using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailGrowerChanger : MonoBehaviour
{

  public Collider collider;

  public TouchBlarp game;

  public float spawnTime;
  public float spawnLength;

  private Material material;

  private float hitTime;
  public float startScale;
  private Collider thisCollider;
  public float lerpScale;

  // Start is called before the first frame update
  void Start()
  {
    material = GetComponent<MeshRenderer>().material;
    Shader.SetGlobalFloat("_TailGrowTime", Time.time);
    thisCollider = GetComponent<Collider>();
  }

  // Update is called once per frame
  void Update()
  {

    float v = (Time.time - spawnTime) / spawnLength;
    if (v > 1)
    {
      Despawn();
    }
    else
    {

      float dV = Mathf.Min(v * 10, (1 - v));
      transform.localScale = Vector3.one * dV * startScale;

      lerpScale = dV;
      if (v > .1f)
      {
        thisCollider.enabled = true;
      }
      else
      {
        thisCollider.enabled = false;
      }

    }
    if (Time.time - spawnTime > spawnLength)
    {
      Despawn();
    }

  }

  void OnTriggerEnter(Collider c)
  {
    if (c.gameObject.tag == "arrow")
    {
      OnHit();
    }
  }

  public void OnHit()
  {
    game.TailGrow();
    Shader.SetGlobalFloat("_TailGrowTime", Time.time);
    Despawn();
  }


  public void Despawn()
  {
    transform.position = Vector3.up * 11;
  }

  public void OnSpawn()
  {

    float wallProblem = (float)game.enemyCollect / 16;
    wallProblem *= .5f;
    float min = 0 + wallProblem;
    float max = 1 - wallProblem;
    min += .1f;
    max -= .1f;

    float wallProblem2 = (float)game.enemyCollect * 9 / (16 * 16);
    wallProblem2 *= .5f;
    float min2 = wallProblem2;
    float max2 = 1 - wallProblem2;
    min2 += .1f;
    max2 -= .1f;

    min = Mathf.Clamp(min, 0, .5f);
    max = Mathf.Clamp(max, 0, .5f);

    min2 = Mathf.Clamp(min2, 0, .5f);
    max2 = Mathf.Clamp(max2, 0, .5f);

    Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width * Random.Range(.1f, .9f), Screen.height * Random.Range(.1f, .9f), 0));
    RaycastHit hit;
    if (collider.Raycast(ray, out hit, 100.0f))
    {
      print(hit.point);
      transform.position = hit.point + Camera.main.transform.forward * -.2f;
      spawnTime = Time.time;
    }
  }


}