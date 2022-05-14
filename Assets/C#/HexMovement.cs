using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexMovement : MonoBehaviour
{
//call an instance of hex and hexmap
public Hex hex;
public HexMap hex_map;
/*
 transforms the hex positions on the map useing the position from camera function 
 from the Hex code along with the hex map Rows and Columns.
*/
    void Update_Map_Position()
    {
        this.transform.position = hex.PositionFromCamera(Camera.main.transform.position,hex_map.Number_Colms,hex_map.Number_Rows);
    }
}
