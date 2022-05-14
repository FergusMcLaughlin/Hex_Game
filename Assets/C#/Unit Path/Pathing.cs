using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Pathing
{
/*
Pathing.Find_Path(ourmap,startPosition);

Tiles will need to return :
1 a list of its neighbours
2 how much the movment cost of the tile is 

*/
public class Pathing 
{
    public static T[] Find_Path<T>(Path_World world, Pathable_Unit unit, T start_tile, 
    T end_tile,Cost_Estimate_Delegate Cost_Estimate_Function )  where T : Path_Interface
    {
        Debug.LogError("Find Path");
        //null value check
        if(world == null || unit == null || start_tile == null || end_tile == null)
        {
            Debug.LogError("null Values sent for path finding");
            return null;
        }

     

    Path_A<T> resolver = new Path_A<T>( world,  unit,  start_tile,  end_tile, Cost_Estimate_Function);

    resolver.Do_Work();

    return resolver.Get_List();

    }
}
public delegate float Cost_Estimate_Delegate(Path_Interface a, Path_Interface b);
}