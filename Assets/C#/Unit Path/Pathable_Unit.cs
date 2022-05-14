using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathing
{
public interface Pathable_Unit
{
    float Cost_to_Enter_Tile(Path_Interface source_tile, Path_Interface destination_tile);
}
}

