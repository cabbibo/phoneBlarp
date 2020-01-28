using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.iOS;

// Input.GetTouch example.
//
// Attach to an origin based cube.
// A screen touch moves the cube on an iPhone or iPad.
// A second screen touch reduces the cube size.

public class TouchArrow : MonoBehaviour
{
    private Vector3 position;
    private float width;
    private float height;

    public Transform touchRep;
    public Collider collider;
    public Transform chaserRep;
    public Material touchMat;


    public Vector3 touchStart;
    public Vector3 touchCurrent;
    public Vector3 touchRelease;

    public Vector3 cPosition;
    public Vector3 cVelocity;
    public Vector3 cForce;

    public GameObject arrowPrefab;
    public GameObject target;


    public LineRenderer lr;

    public bool holding;

    public List<GameObject> arrows;



    void Awake()
    {
        width = (float)Screen.width / 2.0f;
        height = (float)Screen.height / 2.0f;

        touchMat = touchRep.GetComponent<Renderer>().material;
        // Position used for the cube.
        position = new Vector3(0.0f, 0.0f, 0.0f);
        Application.targetFrameRate = 60;
    }

    private Vector3 force;
    private GameObject currentArrow;
    void Update()
    {

        force = Vector3.zero;
        if (Input.GetMouseButtonDown(0))
        {

          GameObject go = Instantiate( arrowPrefab );
          arrows.Add( go );
          currentArrow = go;
          go.GetComponent<ArrowInfo>().time = 1;
             //var ray = Camera.main.ScreenPointToRay (Input.mousePosition);
            //touchRep.position = ray.origin + ray.direction  * 2;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (collider.Raycast(ray, out hit, 100.0f))
            {
                touchRep.position = hit.point;
                touchStart = hit.point;
                touchMat.color = new Color(1,0,0,1);
            }

        }

        if (Input.GetMouseButton(0))
        {

             //var ray = Camera.main.ScreenPointToRay (Input.mousePosition);
            //touchRep.position = ray.origin + ray.direction  * 2;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (collider.Raycast(ray, out hit, 100.0f))
            {
                chaserRep.position = hit.point;
                touchCurrent = hit.point;
                //touchMat.color = new Color(1,0,0,1);
                
                holding = true;
                cPosition = touchCurrent;
                cVelocity = Vector3.zero;

                currentArrow.transform.position = cPosition;


            }

        }else{
        

          if( currentArrow != null ){
          if( holding == true ){

            currentArrow.GetComponent<Rigidbody>().AddForce((touchStart - chaserRep.position) * 100 );
            chaserRep.position = currentArrow.transform.position;

          }



          if( (currentArrow.transform.position-touchStart).magnitude < .7f ){
            holding = false;
          }
        }

        }


        lr.SetPosition( 0 , touchRep.position-Camera.main.transform.forward * .2f);
        lr.SetPosition( 1 , chaserRep.position-Camera.main.transform.forward * .2f);


        int id = 0;
        int removeID = -1;
        foreach(GameObject arrow in arrows ){

          if( arrow != null ){

            arrow.GetComponent<ArrowInfo>().time -= .003f;
            if( arrow.GetComponent<ArrowInfo>().time < 0 ){ removeID = id; }
            arrow.transform.localScale = Vector3.one * arrow.GetComponent<ArrowInfo>().time;

            id ++;

          }

        }

        if( removeID >= 0 ){
          GameObject a = arrows[removeID];
          arrows.RemoveAt( removeID );
          DestroyImmediate( a );
        }


        //force *= .001f;
        //cVelocity += force;
        //cPosition += cVelocity;
        //cVelocity *= .99f;

        //chaserRep.position = cPosition;


    }


}