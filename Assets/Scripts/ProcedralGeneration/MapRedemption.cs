using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class MapRedemption : MonoBehaviour
{
    // map sprites
    [SerializeField] private GameObject Floor;
    [SerializeField] private GameObject Wall;
    
    // Spawner to add to rooms
    [SerializeField] private Spawner spawner;

    // amount of rooms
    public int RoomCount = 100;

    // Max size of the Map
    private int xMax = 80;
    private int yMax = 25;

    // Actual size of Map
    private int _xSize;
    private int _ySize;

    // Set how many objects to generate
    private int _objects = 10;

    // define the chance to build a room
    private int roomChance = 75;

    // create room bool
    private bool createRoom = false;

    // (Phenotype)d
    // Visual representation of data
    private Dictionary<Vector2Int, GameObject> tiles;

    // (Genotype) 
    // Representation of coordinates mapped to data
    private Dictionary<Vector2Int, int> data;

    // this is just to set values to unload later
    private int floorG = 0;
    private int wallG = 1;

    private readonly Action<string> _logger;

    // which direction to move in
    // north: 0, East: 1, South: 2, West: 3
    private int _direction = 0;

    public bool makeRoom(int x, int y, int xLength, int yLength, bool spawn = true )
    {
        // define the dimensions of the room, it should be at least 4x4 tiles (2x2 for walking on, the rest is walls)
        int xlen;
        if ( xLength < 4 )
        {
            xlen = 4;
        }
        else
        {
            xlen = xLength;
        }

        int ylen;
        if ( yLength < 4 )
        {
            ylen = 4;
        }
        else
        {
            ylen = yLength;
        }

        // Check if there is space in Genotype to make room first
        ArrayList coordinates = getRoomCoords(x, y, xlen, ylen);
        foreach (Vector2Int cXY in coordinates)
        {
            if ( isPlaced( cXY ))
            {
                return false;
            }
        }

        // things checked out now can build actual room
        foreach (Vector2Int cXY in coordinates)
        {
            data.Add(cXY, floorG);
        }

        // Put in the spawner in this step to make sure values are proper
        if (spawn)
        {
            spawner.createSpawner(xlen, ylen);
            Instantiate(spawner, new Vector3(x - xlen / 2, y - ylen / 2), Quaternion.identity );
        }

        // made it to end must be true
        return true;
    }

    private ArrayList getRoomCoords(int x, int y, int xLength, int yLength)
    {
        int xL = splitNum( xLength, true );
        int xR = splitNum( xLength, false );
        int yL = splitNum( yLength, true );
        int yR = splitNum( yLength, false );

        ArrayList coords = new ArrayList();

        // Now fill in coordinate array
        int tempyL;
        int tempyR;
        while (xR >= 0)
        {
            tempyL = yL;
            tempyR = yR;
            while (tempyR >= 0)
            {
                coords.Add(new Vector2Int(x + xR, y + tempyR));
                tempyR--;
            }

            while (tempyL > 0)
            {
                coords.Add(new Vector2Int(x + xR, y - tempyL));
                tempyL--;
            }

            xR--;
        }

        while (xL > 0)
        {
            tempyL = yL;
            tempyR = yR;
            while (tempyR >= 0)
            {
                coords.Add(new Vector2Int(x - xL, y + tempyR));
                tempyR--;
            }
            
            while (tempyL > 0)
            {
                coords.Add(new Vector2Int(x - xL, y - tempyL));
                tempyL--;
            }

            xL--;
        }

        return coords;
    }


    private bool makeCooridor(int x, int y, int length, int direction)
    {
        // define the dimensions of the cooridor
        int len = length;
        if (length < 2)
        {
            len = 2;
        }

        int tempx;
        int tempy;
        
        // direction Guide
        // north: 0, East: 1, South: 2, West: 3
        switch ( direction )
        {
            case( 0 ): // North
                tempx = x;
                for (tempy = y; tempy <= (y + len); tempy++)
                {
                    Vector2Int pos = new Vector2Int(tempx, tempy);
                    if (!isPlaced(pos))
                    {
                        data.Add(pos, floorG);
                    }
                }
                break;
            case( 1 ): // East
                tempy = y;
                for (tempx = x; tempx <= ( x + len); tempx++)
                {
                    Vector2Int pos = new Vector2Int(tempx, tempy);
                    if (!isPlaced(pos))
                    {
                        data.Add(pos, floorG);
                    }
                }
                break;
            case( 2 ): // South
                tempx = x;
                for (tempy = y; tempy >= (y - len); tempy--)
                {
                    Vector2Int pos = new Vector2Int(tempx, tempy);
                    if (!isPlaced(pos))
                    {
                        data.Add(pos, floorG);
                    }
                }
                break;
            case( 3 ): // West
                tempy = y;
                for (tempx = x; tempx >= (x - len); tempx--)
                {
                    Vector2Int pos = new Vector2Int(tempx, tempy);
                    if (!isPlaced(pos))
                    {
                        data.Add(pos, floorG);
                    }
                }
                break;
        }
        
        // at end
        return true;
    }
    
    
    // Despawns the level
    private void UnloadTiles()
    {
        // RemoveSpawners();
        
        foreach(Vector2Int coord in tiles.Keys)
        {
            GameObject go = tiles[coord];
            Destroy(go);
        }
        tiles.Clear();
    }
    
    // Remove the Spawners
    private void RemoveSpawners()
    {
        // Destroy all instances of spawners 
        Object[] allObjects = GameObject.FindObjectsOfType( spawner.GetType() );
        foreach( GameObject obj in allObjects) {
            if(obj.transform.name == "Spawner(Clone)"){
                Destroy(obj);
            }
        }
    }

    
    // Despawns the level and then spawns everything based on the most recent data model
    private void LoadTiles()
    {
        UnloadTiles();

        foreach(Vector2Int i in data.Keys)
        {
            GameObject tile;
            if(data[i] == wallG)
            {
                // spawn Wall tile at this location
                tile = Instantiate(Wall, (Vector2)i, Quaternion.identity) as GameObject;
                if (!tiles.ContainsKey(i))
                    tiles.Add(i, tile);
            }
            else
            {
                // spawn Floor tile at this location
                tile = Instantiate(Floor, (Vector2)i, Quaternion.identity) as GameObject;
                if (!tiles.ContainsKey(i))
                    tiles.Add(i, tile);
            }
        }
    }


    private int splitNum(int dimension, bool leftRight)
    {
        // Left is true, Right is false
        if (leftRight)
        {
            return dimension / 2;
        }
        else
        {
            if (dimension % 2 == 0)
            {
                return dimension / 2;
            }
            else
            {
                return dimension / 2;
            }
        }
    }

    private bool isPlaced(Vector2Int key)
    {
        if (data.ContainsKey(key))
        {
            return true;
        }

        return false;
    }
    
    private bool N = true;
    private bool E = true;
    private bool S = true;
    private bool W = true;
    
    private void updateDir( bool Ndir, bool Edir, bool Sdir, bool Wdir )
    {
        N = Ndir;
        E = Edir;
        S = Sdir;
        W = Wdir;
    }
    
    // Entire genotype creation     
    public void GenerateModel()
    {
        // create a room in the center to start from
        makeRoom(0, 0, 7, 7, false);
        
        // keeping track of objects created
        int roomsMade = 1;
        int cooridorsMade = 0;
        
        // room sizes
        int prevXSize = 7;
        int prevYSize = 7;
        int newx = 0;
        int newy = 0;
        
        // Main loop for building other rooms
        for (int attempts = 0; attempts < 1000; attempts++)
        {
            // if the total rooms is reached break
            if ( roomsMade >= _objects)
            {
                break;
            }
            
            // whether a cooridor was built
            bool cooridorbuilt = false;
            
            // pick a random wall to build upon
            int direction = Random.Range(0, 4);
            // pick a random cooridor size
            int cooridorLen = Random.Range(3, 8);

            switch ( direction )
            {
                case( 0 ):
                    // moving North
                    if ( N )
                    {
                        newy = newy + splitNum(prevYSize, false) + 1;
                        makeCooridor(newx, newy, cooridorLen, 0);
                        updateDir( true, true, false, true );
                        cooridorbuilt = true;
                        // update Y location
                        newy = newy + cooridorLen;
                        // update cooridors made
                        cooridorsMade++;
                    }
                    break;
                case( 1 ):
                    // moving East
                    if (E)
                    {
                        newx = newx + splitNum(prevXSize, false ) +1;
                        makeCooridor(newx, newy, cooridorLen, 1);
                        updateDir( true, true, true, false );
                        cooridorbuilt = true;
                        // update X location
                        newx = newx + cooridorLen;
                        // update cooridors made
                        cooridorsMade++;
                    }
                    break;
                case( 2 ):
                    // Moving South
                    if (S)
                    {
                        newy = newy - splitNum( prevYSize, true ) -1;
                        makeCooridor(newx, newy, cooridorLen, 2);
                        updateDir( false, true, true, true );
                        cooridorbuilt = true;
                        // update Y location
                        newy = newy - cooridorLen;
                        // update cooridors made
                        cooridorsMade++;
                    }
                    break;
                case( 3 ):
                    // Moving West
                    if (W)
                    {
                        newx = newx - splitNum( prevXSize, true );
                        makeCooridor(newx, newy, cooridorLen, 3);
                        updateDir( true, false, true, true );
                        cooridorbuilt = true;
                        // update X location
                        newx = newx - cooridorLen;
                        // update cooridors made
                        cooridorsMade++;
                    }
                    break;
            }
            
            // if cooridor was built build a room
            if ( cooridorbuilt )
            {
                int roomYLen = Random.Range(4, 10);
                int roomXLen = Random.Range(4, 10);
                if (roomXLen % 2 == 0)
                {
                    roomXLen++;
                }

                if (roomYLen % 2 == 0)
                {
                    roomYLen++;
                }
                prevXSize = roomXLen;
                prevYSize = roomYLen;
                
                switch( direction )
                {
                    case( 0 ):
                        // Moved North
                        newy = newy + splitNum( prevYSize, true );
                        while( makeRoom(newx, newy +1, roomYLen, roomXLen) );
                        break;
                    case( 1 ):
                        // Moved East
                        newx = newx + splitNum( prevXSize, true );
                        while( makeRoom(newx +1, newy, roomYLen, roomXLen) );
                        break;
                    case( 2 ):
                        // Moved South
                        newy = newy - splitNum( prevYSize, false );
                        while( makeRoom(newx, newy -1, roomYLen, roomXLen) );
                        break;
                    case( 3 ):
                        // Moved West
                        newx = newx - splitNum( prevXSize, false );
                        while( makeRoom(newx -1, newy, roomYLen, roomXLen) );
                        break;
                }
                
                // increase the amount of rooms made
                roomsMade++;
            }
        }


        if ( roomsMade < _objects - 3 || roomsMade < cooridorsMade)
        {
            UnloadTiles();
            GenerateModel();
        }
    }


    public void Awake()
    {
        tiles = new Dictionary<Vector2Int, GameObject>();
        data = new Dictionary<Vector2Int, int>();
    }

    public void Start()
    {
        GenerateModel();
//        makeRoom(0, 0, 4, 4, false);
        LoadTiles();
    }
    
}


