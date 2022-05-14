using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathing;
using System.Linq;


public class Unit : Pathable_Unit
{
    //Unit data declaierd here
    public string Name = "unnamed";
    public int Hit_Points = 100;
    public int Strength = 9;
    public int Movment = 1;
    public int Movment_Pool = 1;
    public Hex Hex {get; protected set;}
//for the event listener
    public delegate void Unit_Moved_Delegate(Hex oldHex, Hex newHex);
    public event Unit_Moved_Delegate On_Unit_Moved;
//the units path it takes hex by hex
    Queue<Hex> Hex_Path;
//move
    const bool TODO_MOVMENT_RULES = true;


    public void Set_Hex(Hex newHex)
    {
       Hex oldHex = Hex;

        if(Hex!=null)
        {
            Hex.Remove_Unit(this);
        }
        Hex = newHex;
        Hex.Add_Unit(this);

        if(On_Unit_Moved != null)
        {
            On_Unit_Moved(oldHex, newHex);
        }
    }

    public void DUMMY_PATH_FUNC()
    {
       Hex[]  hs = Pathing.Pathing.Find_Path<Hex>
       (Hex.HexMap, 
       this, 
       Hex, 
       Hex.HexMap.Get_Hex_At(Hex.Q , Hex.R +2),
        Hex.Cost_Estimate);

        Debug.Log("Size" + hs.Length);

        Set_Hex_Path(hs);
    }

    public void Clear_Hex_Path()
    {
        this.Hex_Path = new Queue<Hex>();
    }

    public void Set_Hex_Path(Hex[] hexArray)
    {
        this.Hex_Path = new Queue<Hex>(hexArray); //sets up new que
       if(Hex_Path.Count == 0)
        {
        this.Hex_Path.Dequeue(); // First gets dequed becuse we are in it already.
       }
    }

    //movement test-----------------------------------------------
    public void Do_Turn()
    {
        Debug.Log("DoTurn");
        //do move que

        if(Hex_Path == null || Hex_Path.Count == 0) // no moves in que
        {
            return;
        }

        Hex oldHex = Hex;
        Hex newHex = Hex_Path.Dequeue();

        //check if the hex is water

        
            Set_Hex(newHex);//if its not water then move there
        
    }

    public int Movment_Costs(Hex hex) //unit cost
    {
        //over ride based on type
        return hex.Movement_Cost();
    }
    public float Turns_Till_Hex(Hex hex, float turns_so_far)
    {
        float base_turns_to_enter_hex = Movment_Costs(hex) / Movment;
        if (base_turns_to_enter_hex < 0)
        {
            //impassible
            Debug.Log("cant pass " + hex.ToString());
            return -99;
        }

        if(base_turns_to_enter_hex > 1)
        {
            base_turns_to_enter_hex = 1; //if something is expensive to enter you can still enter on one turn.
        }

        float turns_left = Movment_Pool / Movment; //ERROR ON POOL ??

        float turns_so_far_whole = Mathf.Floor(turns_so_far); // rounds the turns
        float turns_so_far_fraction = turns_so_far - turns_so_far_whole; //factored in with base turms for hex entry

        if(turns_so_far_fraction < 0 && turns_so_far_fraction < 0.01f || turns_so_far_fraction > 0.99f) //in the case of 0.33333* up to its useal tricks
        {
            Debug.LogError("0.3333 up to no go");
            //Round up
            if (turns_so_far_fraction < 0.01f)
            {
                turns_so_far_fraction = 0;
            }
            if (turns_so_far_fraction > 0.99f)
            {
                turns_so_far_whole += 1;
                turns_so_far_fraction = 0;
            }
        }

        float turns_used_after_move = turns_so_far_fraction + base_turns_to_enter_hex;

        if (turns_used_after_move > 1)
        {
            //not enough movment to complet move
            if (TODO_MOVMENT_RULES)
            {
                if(turns_so_far_fraction == 0)
                {
                    //tile to expensive to enter
                }
                else
                {
                turns_so_far_whole += 1;
                turns_so_far_fraction = 0;
                }
                // we will know here its a new turn
                turns_used_after_move = base_turns_to_enter_hex;
                if(turns_used_after_move > 1)
                {
                    turns_used_after_move =1;
                }

            } 
            else
            {
                //can always enter tiles 
                turns_used_after_move = 1;
            }
        } 


        return turns_so_far_whole + turns_used_after_move;

    }
//look at this again
   public  float Cost_to_Enter_Tile (Path_Interface source_tile, Path_Interface destination_tile)
    {
        return 1;
    }

}