using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexMap_IRE : HexMap
{
override public void GenerateMap()
{
    //gnerate the blank map
    base.GenerateMap();
    //This should generate an ireland looking map for testing
    for(int x=12;x!=21;x++){
    ChangeAreaGrass(x,28,1);
    }
    for(int x=13;x!=17;x++){
    ChangeAreaGrass(x,29,1);
    }
    for(int x=13;x!=15;x++){
    ChangeAreaGrass(x,30,1);
    }
    for(int x=12;x!=22;x++){
    ChangeAreaGrass(x,27,1);
    }
    for(int x=12;x!=23;x++){
    ChangeAreaGrass(x,26,1);
    }
    for(int x=16;x!=23;x++){
    ChangeAreaGrass(x,25,1);
    }
    for(int x=18;x!=22;x++){
    ChangeAreaGrass(x,24,1);
    }

    ChangeAreaGrass(13,13,1);
    ChangeAreaGrass(13,32,1);
    ChangeAreaGrass(12,29,1);
    ChangeAreaGrass(12,30,1);

    ChangeAreaWater(21,26,1);
    ChangeAreaWater(15,26,1);
    ChangeAreaWater(20,24,1);
    ChangeAreaWater(20,24,1);

    ChangeAreaWood(20,27,1);
    ChangeAreaWood(18,26,1);
    ChangeAreaWood(17,27,1);
    ChangeAreaWood(14,29,1);
    ChangeAreaWood(17,26,1);
    ChangeAreaWood(16,27,1);
    ChangeAreaWood(18,27,1);
    ChangeAreaWood(19,27,1);
    ChangeAreaWood(19,26,1);
    ChangeAreaWood(17,28,1);

    ChangeAreaHill(18,25,1);
    ChangeAreaHill(17,25,1);
    ChangeAreaHill(19,28,1);
    ChangeAreaHill(13,26,1);
    ChangeAreaHill(13,27,1);
    ChangeAreaHill(22,26,1);
    ChangeAreaHill(21,24,1);
    ChangeAreaHill(19,24,1);
    ChangeAreaHill(19,25,1);
    ChangeAreaHill(12,27,1);
    ChangeAreaHill(14,28,1);


    ChangeAreaMarsh(22,25,1);
    ChangeAreaMarsh(16,26,1);
    ChangeAreaMarsh(15,27,1);
    ChangeAreaMarsh(16,28,1);
    ChangeAreaMarsh(20,26,1);
    ChangeAreaMarsh(13,29,1);

    //Call update to change the sprites
    UpdateHex();

    Unit unit = new Unit();
    Spawn_Unit_At(unit,Unit_Object,13,27);
}
    void ChangeAreaGrass (int r, int q, int radius)
    {
        Hex centerHex = Get_Hex_At(q,r);
        centerHex.Type = 1;
    }
    void ChangeAreaWater (int r, int q, int radius)
    {
        Hex centerHex = Get_Hex_At(q,r);
        centerHex.Type = 0;
    }
        void ChangeAreaHill (int r, int q, int radius)
    {
        Hex centerHex = Get_Hex_At(q,r);
        centerHex.Type = 2;
    }
        void ChangeAreaMarsh (int r, int q, int radius)
    {
        Hex centerHex = Get_Hex_At(q,r);
        centerHex.Type = 3;
    }
        void ChangeAreaWood (int r, int q, int radius)
    {
        Hex centerHex = Get_Hex_At(q,r);
        centerHex.Type = 4;
    }
}
