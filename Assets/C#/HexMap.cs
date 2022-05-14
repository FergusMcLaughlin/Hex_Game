using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathing;
using System.Linq;

    


public class HexMap : MonoBehaviour, Path_World
{
    // Start is called before the first frame update
    void Start()
    {
        GenerateMap();

      //  hexMap = GameObject.FindObjectOfType<HexMap>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.D))
        {
            if(units != null)
            {
                foreach (Unit u in units)
                {
                    u.DUMMY_PATH_FUNC();
                }
            }
        }

        
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(units != null)
            {
                foreach (Unit u in units)
                {
                    u.Do_Turn();
                }
            }
        }
      
    }
    //hex and unit objects (add in inspector)

    public GameObject Hex_Object;
    public GameObject Unit_Object;

    //Terrain Types (add in inspector)
    public Sprite Water;
    public Sprite Grass;
    public Sprite Hills;
    public Sprite Marsh;
    public Sprite Woods;

    //wrapping controls
    public bool AllowWrapEastWest = true;
    public bool AllowWrapNorthSouth = false;

    //hex dictionary
    private Hex[,] tiles;
    private Dictionary<Hex, GameObject> hexToGameObjectMap;
    private Dictionary<GameObject, Hex> gameObjectToHexMap;



    //unit hashset
    private HashSet<Unit> units;
    private Dictionary<Unit, GameObject> unitToGameObjectMap;

    //Number of Rows and Cols (Map Size)
    public readonly int Number_Colms = 50;
    public readonly int Number_Rows = 35;

//returns the hex at its x and y position
    public Hex Get_Hex_At(int x, int y)
    {
        if(tiles== null) //makes sure map has been created
        {
            Debug.LogError("tiles dont exist");
            return null;
        }
         if(AllowWrapEastWest)
        {
            x = x % Number_Rows;
            if(x < 0)
            {
                x += Number_Rows;
            }
        }
        if(AllowWrapNorthSouth)
        {
            y = y % Number_Colms;
            if(y < 0)
            {
                y += Number_Colms;
            }
        }

        try {
            return tiles[x, y];
        }
        catch
        {
            Debug.LogError("GetHexAt: " + x + "," + y);
            return null;
        }
    }

    public Hex GetHexFromGO(GameObject tileGO)
    {
        Debug.Log( tileGO ); // function gets hex
        
        if(gameObjectToHexMap.ContainsKey(tileGO))///problem is with dictionary
        {
            return gameObjectToHexMap[tileGO];
        } else{
        Debug.Log("Nothing");
        return null;
    }
    }
    public GameObject GetTileGO(Hex tile)
    {
        if( hexToGameObjectMap.ContainsKey(tile) )
        {
            return hexToGameObjectMap[tile];
        }

        return null;
    }

//Stuff added for the unit movment
    public Vector3 Get_Hex_Position(int q, int r)
    {
        Hex hex = Get_Hex_At(q,r);
        return Get_Hex_Position(hex);
    }

    public Vector3 Get_Hex_Position(Hex hex)
    {
        return hex.PositionFromCamera(Camera.main.transform.position,Number_Rows,Number_Colms);
    }
    
    /*
    public GameObject GettileGO(Hex tile)
    {
        if( hexToGameObjectMap.ContainsKey(tile) )
        {
            return hexToGameObjectMap[tile];
        }
     return null;
    }
*/
    virtual public void GenerateMap()
    {
        tiles = new Hex[Number_Colms,Number_Rows];
        hexToGameObjectMap = new Dictionary<Hex, GameObject>();
        gameObjectToHexMap = new Dictionary<GameObject, Hex>();

        //generates the hex map
        for (int col = 0; col < Number_Colms; col++)
        {
        for (int row = 0; row < Number_Rows; row++)
        {
            Hex tile = new Hex (this, col,row);
            tile.Type = -1; //sets type to water by defualt
            tiles[col,row] = tile;

            Vector3 pos = tile.PositionFromCamera(Camera.main.transform.position,Number_Rows,Number_Colms);
            //instansiate hex as a game objects
            GameObject tileGO = (GameObject)Instantiate(
                    Hex_Object, 
                    pos,
                    Quaternion.identity,
                    this.transform
                );
            //adds the tile info and game object to the hex dictionary
           // hexToGameObjectMap.Add(tile, tileGO);_
            hexToGameObjectMap[tile] = tileGO;
            gameObjectToHexMap[tileGO] = tile;
            //makes them a sub object of the HexMap and names them "Hex(1,1)" as they are created
            tileGO.name = "Hex("+row+","+col+")";
            tileGO.transform.SetParent(transform);
            //GOBACKANDCHECKTHIS
            tileGO.GetComponent<HexMovement>().hex = tile;
            tileGO.GetComponent<HexMovement>().hex_map = this;

        }
        }
        //after initiateing this call an update.
        UpdateHex();
    }


    public void UpdateHex()
    {
        for (int col = 0; col < Number_Colms; col++)
        {
            for (int row = 0; row < Number_Rows; row++)
            {
                Hex tile = tiles[col,row];
                GameObject tileGO = hexToGameObjectMap[tile];

                //attach the sprite
                SpriteRenderer sr1 = tileGO.GetComponentInChildren< SpriteRenderer>();
                sr1.sprite = Water;

                tile.movment_cost = 2;
                /*
                dont like this system try and come up with another 
                its to big and combersum, maybe and array to check it agenst ?
                */
                if(tile.Type  == 1 )
                {
                SpriteRenderer sr2 = tileGO.GetComponentInChildren< SpriteRenderer>();
                sr2.sprite = Grass;
                tile.movment_cost = 1;


                } 
                else if(tile.Type  == 2 )
                {
                SpriteRenderer sr3 = tileGO.GetComponentInChildren< SpriteRenderer>();
                sr3.sprite = Hills;
                tile.movment_cost = 3;
                }
                else if(tile.Type  == 3 )
                {
                SpriteRenderer sr4 = tileGO.GetComponentInChildren< SpriteRenderer>();
                sr4.sprite = Marsh;
                tile.movment_cost = 4;
                }
                else if(tile.Type  == 4 )
                {
                SpriteRenderer sr5 = tileGO.GetComponentInChildren< SpriteRenderer>();
                sr5.sprite = Woods;
                tile.movment_cost = 2;
                }
                 else if(tile.Type  == 0 )//change
                {
                SpriteRenderer sr6 = tileGO.GetComponentInChildren< SpriteRenderer>();
                sr6.sprite = Water;
                tile.movment_cost = -99;

                }
            }
        }
    }

    public void Spawn_Unit_At(Unit unit, GameObject prefab, int r, int q)
    {
        if (units == null)
        {
            units = new HashSet<Unit>();
            unitToGameObjectMap = new Dictionary<Unit,GameObject>();
        }

    //the hex the unit will spawn at and be a child of.
        Hex thisHex = Get_Hex_At(q, r);
        GameObject selectedHex = hexToGameObjectMap[thisHex];
        unit.Set_Hex(thisHex);
    // finds the actual position of the hex adds the unit to the hex as a child class
    //not transforming properly
    
        GameObject unitGO = (GameObject)Instantiate(prefab, selectedHex.transform.position, Quaternion.identity, selectedHex.transform); 
        //adds to the unit moved list can take awat BUT CANT JUST ASSIGN
        unit.On_Unit_Moved += unitGO.GetComponent<UnitView>().On_Unit_Moved;

       // unit.On_Unit_Moved(thisHex,thisHex); //TEMP

      units.Add(unit);
      unitToGameObjectMap.Add(unit, unitGO);
    }
    
}
