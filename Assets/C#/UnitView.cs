using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitView : MonoBehaviour
{
    void Start()
    {
        Old_Position = New_Position = this.transform.position;

        New_Position = this.transform.position + Vector3.right;
    }
    Vector3 Old_Position;
    Vector3 New_Position;
    Vector3 currentVelocity;
    float Speed = 0.3f;

public void On_Unit_Moved(Hex oldHex, Hex newHex)
{

//checks for water and stops movment
   if(newHex.Type <= -1)
   {
       Debug.Log("water");
       return;
   } else{
       this.transform.position = oldHex.PositionFromCamera();
       New_Position = newHex.PositionFromCamera();
       currentVelocity = Vector3.zero;

   Old_Position = oldHex.PositionFromCamera();
    New_Position = newHex.PositionFromCamera();
   }
}

void Update()
{
    this.transform.position = Vector3.SmoothDamp(this.transform.position, New_Position, ref currentVelocity, Speed); // unit flying away problem here
}
}
