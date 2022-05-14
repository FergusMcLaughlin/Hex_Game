using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//alows the array to list in unit array
using System.Linq;
using Pathing;
//hex class
public class Hex : Path_Interface
{
    //Hex Info
public Hex(HexMap hexMap, int q, int r) 
    {
        this.HexMap = hexMap;
        this.Q = q;
        this.R = r;
        //remove if it  works
        this.S = -(q+r) ;//dont think this is really needed, mabey for 3d

         units = new HashSet<Unit>();

    }
    //hex co-ords
public readonly int Q; //col
public readonly int R; //row
public readonly int S;
//ref to hex map
public readonly HexMap HexMap;
    //Map Data
    //Maybe look at this and not use ints
public int Type = -1;
public int movment_cost = 1;
  //public int Tag

    static readonly float Width_Multiplier = Mathf.Sqrt(3)/2; //LOOK AT THIS WHAT DO IT DO ?????
    float radius = 0.329f; //size of sprites spacing 
//list of units in this hex


// returns hex's position
  public Vector3 Position()
  {
      return new Vector3(Row_spaceing()*(this.Q+this.R/2f),Col_spaceing()*this.R);
  }
//hex height and width
public float Hex_Height()
{
    return radius * 2;
}
public float Hex_Width()
{
    return Width_Multiplier * Hex_Height();
}
//col and row spaceing
public float Col_spaceing()
{
    return Hex_Height() * 0.75f;
}
public float Row_spaceing()
{
    return Hex_Width();
}
//returns the hex map position
public Vector3 PositionFromCamera()
{
    return HexMap.Get_Hex_Position(this);
}

//camera wrapping left to right
public Vector3 PositionFromCamera(Vector3 Camera_Position, float Number_Colms, float Number_Rows)
{
    float Map_Width = Number_Colms * Row_spaceing(); //map width
    float Map_Height  = Number_Rows * Row_spaceing();

    Vector3 position = Position();

 if(HexMap.AllowWrapEastWest)
        {
            float howManyWidthsFromCamera = Mathf.Round((position.x - Camera_Position.x) / Map_Width);
            int howManyWidthToFix = (int)howManyWidthsFromCamera;

            position.x -= howManyWidthToFix * Map_Width;
        }

        if(HexMap.AllowWrapNorthSouth)
        {
            float howManyHeightsFromCamera = Mathf.Round((position.z - Camera_Position.z) / Map_Height);
            int howManyHeightsToFix = (int)howManyHeightsFromCamera;

            position.z -= howManyHeightsToFix * Map_Height;
        }


        return position;
    }
public static float Cost_Estimate(Path_Interface a, Path_Interface b)
{
    return Distance((Hex)a, (Hex) b);
}

//not really used, just here for the cost estimate
public static float Distance(Hex a, Hex b)
    {
        // WARNING: Probably Wrong for wrapping
        int dQ = Mathf.Abs(a.Q - b.Q);
        if(a.HexMap.AllowWrapEastWest)
        {
            if(dQ > a.HexMap.Number_Colms / 2)
                dQ = a.HexMap.Number_Colms - dQ;
        }

        int dR = Mathf.Abs(a.R - b.R);
        if(a.HexMap.AllowWrapNorthSouth)
        {
            if(dR > a.HexMap.Number_Rows / 2)
                dR = a.HexMap.Number_Rows - dR;
        }

        return 
            Mathf.Max( 
                dQ,
                dR,
                Mathf.Abs(a.S - b.S)
            );
    }

//adds a unit to a hex
public void Add_Unit(Unit unit)
{
    if(units == null) // if no units in it before, make a new list
    {
        units = new HashSet<Unit>();
    }
    units.Add(unit);
}
//removes a unit from a hex
public void Remove_Unit(Unit unit)
{
    if(units != null) //checks there are units to remove
    {
    units.Remove(unit);
    }
}
HashSet<Unit> units;
//unit array
public Unit[] Units{
get{
    return units.ToArray();
}
}

public int Movement_Cost()
{
    //factor in hex type 
    return 1;
}
//used to save the current neighbours so u dont have to check all the timne
Hex[] neighbors;

public Path_Interface[] Get_Neighbours()
{
    if (this.neighbors != null)
    {
        return this.neighbors;
    }
    
    List<Hex> neighbors = new List<Hex>();
 //all sides of the hex
 neighbors.Add(HexMap.Get_Hex_At(Q + 1,R + 0));
 neighbors.Add(HexMap.Get_Hex_At(Q + -1,R + 0));
 neighbors.Add(HexMap.Get_Hex_At(Q + 1,R + 1));
 neighbors.Add(HexMap.Get_Hex_At(Q + 1,R + -1));
 neighbors.Add(HexMap.Get_Hex_At(Q + 1,R + -1));
 neighbors.Add(HexMap.Get_Hex_At(Q + -1,R + 1));

 List<Hex>neighbors2 = new List<Hex>();
//check for nulls like edges of the map
 foreach (Hex h in neighbors)
 {
     if(h != null)
     {
         neighbors2.Add(h);
     }
 }
 this.neighbors = neighbors2.ToArray();
 return this.neighbors;
}

public float Cost_to_Enter(float cost_so_far, Path_Interface source_tile, Pathable_Unit selected_unit )
{
    //no source tile needs added
    return ((Unit)selected_unit).Turns_Till_Hex(this, cost_so_far);
}

}
