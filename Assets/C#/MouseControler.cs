using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControler : MonoBehaviour
{
    public float offset;
    public float speed;
    //x - min y - max
    public Vector2 minMaxXPosition;
    public Vector2 minMaxYPosition;
    private float screenWidth;
    private float screenHeight;
    private Vector3 cameraMove;
    HexMap hexMap;
    // Use this for initialization
    void Start()
    {
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        cameraMove.x = transform.position.x;
        cameraMove.y = transform.position.y;
        cameraMove.z = transform.position.z;

        Update_Current_Function = Update_Detect_Start_Mode;
        

        hexMap = GameObject.FindObjectOfType<HexMap>();
      //  Debug.Log( "found  " + hexMap );

    }
 
    delegate void Update_Function();
    Update_Function Update_Current_Function;

    
    Hex hexUnderMouse;
    Hex hexLastUnderMouse;
    //Variables for mouse actions
    Vector3 Last_Mouse_Position; //from Imput.mosue position(actual mouse pixles)

    //Unit movment
    Unit Selected_Unit = null;
    Hex[] hexPath;

    public LayerMask Layer_ID_Hex;
   // Update is called once per frame
    void Update()
    {
        Update_Camera();

       // hexUnderMouse = Mouse_to_Hex(); 

       if(Input.GetKeyDown(KeyCode.Escape) ) // esc key cancles anything
       {
           Cancle_Update_Function();
        }

        Update_Current_Function();

      Last_Mouse_Position = Input.mousePosition;

      hexLastUnderMouse = hexUnderMouse;



    }
    public void Cancle_Update_Function()
    {
        Update_Current_Function = Update_Detect_Start_Mode;

        // Also do cleanup of any UI stuff associated with modes.
        hexPath = null;
    }

    void Update_Detect_Start_Mode()
    {
       //Left mouse button down.(only returns true once)
       if(Input.GetMouseButtonDown(0))
       {
           Debug.Log("MOUSE DOWN");
       }
       else if (Input.GetMouseButtonUp(0))
       {
         Debug.Log("CLICK");
       Mouse_to_Hex();

 //          Unit[] us = hexUnderMouse.Units;//hex under mouse "due to function in question"

 //          if (us.Length > 0)
 //          {
 //              Selected_Unit = us[0];
 //              Update_Current_Function = Update_Unit_Movment;
 //          }

       }
       else if (Selected_Unit!= null && Input.GetMouseButton(1))
       {
           //wehave selected unit and mouse button is down, unit movment mode show path from unit to mouse position
       }
    /*   else if()
       {

       }
       else if()
       {
        }
    */   
    }

    Hex Mouse_to_Hex(){
      
    int layerMask = Layer_ID_Hex.value;
     Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
     RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero,layerMask);

        if(hit != false)
        {
        //Debug.Log( hit.rigidbody.gameObject.name );

        GameObject tileGO = hit.rigidbody.gameObject; // creates sucsessfully, mybe not a game object ?
        Debug.Log( tileGO);
        return hexMap.GetHexFromGO(tileGO); // sucsessfully passes to function
        

        }
          Debug.Log( "Northing" );
         return null;
        
   
 //       if(hit.collider != null)
 //       {
  //          Debug.Log( hit.collider.name );
  //          GameObject tileGO = hit.collider.gameObject;
  //          return hexMap.GetHexFromGameObject(tileGO);
  //      }
  //      Debug.Log( "Northing" );
  //      return null;
   

             /*  
    int layerMask = Layer_ID_Hex.value;
    RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero,layerMask);
 
    if(hit.collider != null)
{
  //  Debug.Log ("Target Position: " + hit.collider.name);
    GameObject tileGO = hit.rigidbody.gameObject;
    return hexMap.Get_Hex_From_Game_Object(tileGO);
}
    return null;
    
      /* /////////////////
         Vector2 mouseRay = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    int layerMask = Layer_ID_Hex.value;
    RaycastHit2D hitInfo = Physics2D.Raycast(mouseRay,Vector2.zero,Mathf.Infinity,layerMask);
    if(hitInfo == true)
    {
    //Debug.Log ("Target Position: " + hit.collider.name);
    GameObject tileGO = hitInfo.collider.gameObject;//rb
    return hexMap.GetHexFromGameObject(tileGO);
    }
    return null;
        */
        

     /*   Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        int layerMask = Layer_ID_Hex.value;

        if( Physics.Raycast(mouseRay, out hitInfo, Mathf.Infinity, layerMask) )
        {
            //hit
            Debug.Log( hitInfo.collider.name );

            GameObject tileGO = hitInfo.rigidbody.gameObject;


            return hexMap.Get_Hex_From_Game_Object(tileGO);
        }

        return null;*/
    }
    Vector3 MouseToGroundPlane(Vector3 mousePos) // possible Junk
    {
        Ray mouseRay = Camera.main.ScreenPointToRay (mousePos);
        // What is the point at which the mouse ray intersects Y=0
        if (mouseRay.direction.y >= 0) {
            //Debug.LogError("Why is mouse pointing up?");
            return Vector3.zero;
        }
        float rayLength = (mouseRay.origin.y / mouseRay.direction.y);
        return mouseRay.origin - (mouseRay.direction * rayLength);
    }

    void Update_Unit_Movment()
    {
        //if cancled
        if(Input.GetMouseButtonUp(1) || Selected_Unit == null)
        {
            Debug.Log("UNITMODE");
            if (Selected_Unit != null)
            {
            Selected_Unit.Set_Hex_Path(hexPath);
            }
            //copy path to unit movment que

            Cancle_Update_Function();
            return;
        }
        //look for the hex the mouse is over, draw a path to it
        if(hexPath == null || hexUnderMouse != hexLastUnderMouse)
        {
         hexPath = Pathing.Pathing.Find_Path<Hex>(hexMap, Selected_Unit, Selected_Unit.Hex, hexUnderMouse, Hex.Cost_Estimate);
        }


    }

    

    void Update_Camera()
    {


        //Move camera
        if ((Input.mousePosition.x > screenWidth - offset) && transform.position.x < minMaxXPosition.y)
        {
            cameraMove.x += MoveSpeed();
           // Debug.Log("Right");
        }
        if ((Input.mousePosition.x < offset) && transform.position.x > minMaxXPosition.x)
        {
            cameraMove.x -= MoveSpeed();
           // Debug.Log("Left");
        }
        if ((Input.mousePosition.y > screenHeight - offset) && transform.position.y < minMaxYPosition.y)
        {
            cameraMove.y += MoveSpeed();
           // Debug.Log("Up");
        }
        if ((Input.mousePosition.y < offset) && transform.position.y > minMaxYPosition.x)
        {
            cameraMove.y -= MoveSpeed();
           // Debug.Log("Down");
        }
        transform.position = cameraMove;
    }
    float MoveSpeed()
    {
        return speed * Time.deltaTime;
    }
}

