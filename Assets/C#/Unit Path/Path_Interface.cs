using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathing
{
    public interface Path_Interface
    {
        Path_Interface[] Get_Neighbours();

        float Cost_to_Enter(float cost_so_far, Path_Interface source_tile, Pathable_Unit selected_unit );



    }
}