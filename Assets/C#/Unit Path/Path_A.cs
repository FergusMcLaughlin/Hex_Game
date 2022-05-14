using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/*
 Followed a tutorial for the Pathing, Most of the used pathing stuff is taken from 
https://www.youtube.com/watch?v=t5x8SM6Ho5M&list=PLbghT7MmckI7JHf0pdEQ8fbPb-LoDXEno&index=16
and based on the A* Movment Algorythem : https://en.wikipedia.org/wiki/A*_search_algorithm
*/

namespace Pathing
{

public class Path_A<T> where T : Path_Interface
{
   public Path_A (Path_World world, Pathable_Unit unit, T start_tile, T end_tile, Cost_Estimate_Delegate Cost_Estimate_Function)
   {
       this.world = world;
       this.unit = unit;
       this.start_tile = start_tile;
       this.end_tile = end_tile;
       this.Cost_Estimate_Function = Cost_Estimate_Function;


   }
    Path_World world;
    Pathable_Unit unit;
    T start_tile;
    T end_tile;
    Cost_Estimate_Delegate Cost_Estimate_Function = null;

   Queue<T> path;

   public void Do_Work()
   {
   //    Debug.LogError("Do Work");
       path = new Queue<T>();

       HashSet<T> closed_set = new HashSet<T>();

       PathfindingPriorityQueue<T> open_set = new PathfindingPriorityQueue<T>();
    //  open_set.Enqueue(end_tile, 99); uses a binary tree. end tile is the hex and the number is the priority 
    open_set.Enqueue(start_tile, 0);//enter start tiel distance from self is 0

    Dictionary<T, T> comeing_from = new Dictionary<T, T>(); // infor on where we came from
//represent the cost of the path, g = cost from a to b, f = estimated cost of getting to the tile. A star uses this in its estimate
    Dictionary<T, float> g_score = new Dictionary<T, float>();
    g_score[start_tile] = 0;

    Dictionary<T, float> f_score = new Dictionary<T, float>();
    f_score[start_tile] = Cost_Estimate_Function(start_tile, end_tile);

     while (open_set.Count > 0)
            {
                T current = open_set.Dequeue();

                // Check to see if we are there.
                if ( System.Object.ReferenceEquals( current, end_tile ) )
                {
                    Reconstruct_path(comeing_from, current);
                    return;
                }

                closed_set.Add(current);

                foreach (T edge_neighbour in current.Get_Neighbours())
                {
                    T neighbour = edge_neighbour;

                    if (closed_set.Contains(neighbour))
                    {
                        continue; // ignore this already completed neighbor
                    }

                    float total_pathfinding_cost_to_neighbor = 
                        neighbour.Cost_to_Enter( g_score[current], current, unit );

                    if(total_pathfinding_cost_to_neighbor < 0)
                    {
                        // Values less than zero represent an invalid/impassable tile
                        continue;
                    }
                    //Debug.Log(total_pathfinding_cost_to_neighbor);

                    float tentative_g_score = total_pathfinding_cost_to_neighbor;

                    // Is the neighbour already in the open set?
                    //   If so, and if this new score is worse than the old score,
                    //   discard this new result.
                    if (open_set.Contains(neighbour) && tentative_g_score >= g_score[neighbour])
                    {
                        continue;
                    }

                    // This is either a new tile or we just found a cheaper route to it
                    comeing_from[neighbour] = current;
                    g_score[neighbour] = tentative_g_score;
                    f_score[neighbour] = g_score[neighbour] + Cost_Estimate_Function(neighbour, end_tile);

                    open_set.EnqueueOrUpdate(neighbour, f_score[neighbour]);
                } // foreach neighbour
            } // while

   }
   private void Reconstruct_path(Dictionary<T, T> comeing_from,T current)
        {
            // So at this point, current IS the goal.
            // So what we want to do is walk backwards through the comeing_from
            // map, until we reach the "end" of that map...which will be
            // our starting node!
            Queue<T> total_path = new Queue<T>();
            total_path.Enqueue(current); // This "final" step is the path is the goal!

            while (comeing_from.ContainsKey(current))
            {
                /*    comeing_from is a map, where the
            *    key => value relation is real saying
            *    some_node => we_got_there_from_this_node
            */

                current = comeing_from[current];
                total_path.Enqueue(current);
            }

            // At this point, total_path is a queue that is running
            // backwards from the END tile to the START tile, so let's reverse it.
            path = new Queue<T>(total_path.Reverse());
        }


   public T[] Get_List()
   {
       return path.ToArray();
   }

}
}
