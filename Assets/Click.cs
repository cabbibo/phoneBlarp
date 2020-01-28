using UnityEngine;
using System.Collections;
using UnityEngine.iOS;

// Input.GetTouch example.
//
// Attach to an origin based cube.
// A screen touch moves the cube on an iPhone or iPad.
// A second screen touch reduces the cube size.

public class Click : MonoBehaviour
{
    private Vector3 position;
    private float width;
    private float height;

    public Transform touchRep;
    public Collider collider;
    public Transform chaserRep;
    public Material touchMat;

    public Vector3 cPosition;
    public Vector3 cVelocity;
    public Vector3 cForce;

    void Awake()
    {
        width = (float)Screen.width / 2.0f;
        height = (float)Screen.height / 2.0f;

        touchMat = touchRep.GetComponent<Renderer>().material;
        // Position used for the cube.
        position = new Vector3(0.0f, 0.0f, 0.0f);
    }

    private Vector3 force;
    void Update()
    {

        force = Vector3.zero;
        if (Input.GetMouseButton(0))
        {


            //var ray = Camera.main.ScreenPointToRay (Input.mousePosition);
            //touchRep.position = ray.origin + ray.direction  * 2;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (collider.Raycast(ray, out hit, 100.0f))
            {
                touchRep.position = hit.point;
                touchMat.color = new Color(1,0,0,1);
                force += touchRep.position - chaserRep.position;
            }
            //print( Camera.main.ScreenToWorldPoint( new Vector3( Input.mousePosition.x , Input.mousePosition.y , Camera.main.nearClipPlane ) );
           
            //Debug.Log("Pressed left click.");
        }else{
          // touchRep.position = new Vector3(10000 , 0,0);
           touchMat.color = new Color(0,1,0,1); 
        }


        force *= .001f;
        cVelocity += force;
        cPosition += cVelocity;
        cVelocity *= .99f;

        chaserRep.position = cPosition;


    }


}