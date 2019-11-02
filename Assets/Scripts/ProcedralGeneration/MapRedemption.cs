using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;
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
        int xL;
        int xR;
        int yL;
        int yR;

        ArrayList coords = new ArrayList();

        if (xLength % 2 == 0)
        {
            // is even split
            xL = xLength / 2;
            xR = xLength / 2;
        }
        else
        {
            xL = xLength / 2;
            xR = (xLength / 2) + 1;
        }

        if (yLength % 2 == 0)
        {
            // is even split
            yL = yLength / 2;
            yR = yLength / 2;
        }
        else
        {
            yL = yLength / 2;
            yR = (yLength / 2) + 1;
        }

        // Now fill in coordinate array
        int tempyL;
        int tempyR;
        while (xL >= 0)
        {
            tempyL = yL;
            tempyR = yR;
            while (tempyL >= 0)
            {
                coords.Add(new Vector2Int(x - xL, y + tempyL));
                tempyL--;
            }

            while (tempyR > 0)
            {
                coords.Add(new Vector2Int(x - xL, y - tempyR));
                tempyR--;
            }

            xL--;
        }

        while (xR > 0)
        {
            tempyL = yL;
            tempyR = yR;
            while (tempyL >= 0)
            {
                coords.Add(new Vector2Int(x + xR, y + tempyL));
                tempyL--;
            }
            
            while (tempyR > 0)
            {
                coords.Add(new Vector2Int(x + xR, y - tempyR));
                tempyR--;
            }

            xR--;
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
        int tempy = 0;
        
        // direction Guide
        // north: 0, East: 1, South: 2, West: 3
        switch ( direction )
        {
            case( 0 ): // North
                tempx = x;
                for (tempy = y; tempy > (y - len); tempy--)
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
                for (tempx = x; tempx < ( x + len); tempx++)
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
                for (tempy = y; tempy < (y + len); tempy++)
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
                for (tempx = x; tempx > (x - len); tempx--)
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
        foreach(Vector2Int coord in tiles.Keys)
        {
            GameObject go = tiles[coord];
            Destroy(go);
        }
        tiles.Clear();
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
                return dimension / 2 + 1;
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
        makeRoom(0, 0, 6, 6, false);
        
        // keeping track of objects created
        int featuresMade = 1;
        
        int prevXSize = 6;
        int prevYSize = 6;
        int newx = 0;
        int newy = 0;
        
        // Main loop for building other rooms
        for (int attempts = 0; attempts < 1000; attempts++)
        {
            // if the total rooms is reached break
            if (featuresMade >= _objects)
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
                        newy = newy + prevYSize / 2;
                        makeCooridor(newx, newy, cooridorLen, 0);
                        updateDir( true, true, false, true );
                        cooridorbuilt = true;
                    }
                    break;
                case( 1 ):
                    // moving East
                    if (E)
                    {
                        newx = newx + prevXSize / 2;
                        makeCooridor(newx, newy, cooridorLen, 1);
                        updateDir( true, true, true, false );
                        cooridorbuilt = true;
                    }
                    break;
                case( 2 ):
                    // Moving South
                    if (S)
                    {
                        newy = newy - prevYSize / 2;
                        makeCooridor(newx, newy, cooridorLen, 2);
                        updateDir( false, true, true, true );
                        cooridorbuilt = true;
                    }
                    break;
                case( 3 ):
                    // Moving West
                    if (W)
                    {
                        newx = newx - prevXSize / 2;
                        makeCooridor(newx, newy, cooridorLen, 3);
                        updateDir( true, false, true, true );
                        cooridorbuilt = true;
                    }
                    break;
            }
            
            // if cooridor was built build a room
            if ( cooridorbuilt )
            {
                int roomYLen = Random.Range(4, 9);
                int roomXLen = Random.Range(4, 9);
                prevXSize = roomXLen;
                prevYSize = roomYLen;
                
                switch( direction )
                {
                    case( 0 ):
                        // Moved North
                        newy = newy + prevYSize /2;
                        makeRoom(newx, newy, roomYLen, roomXLen);
                        break;
                    case( 1 ):
                        // Moved East
                        newx = newx + prevXSize /2;
                        makeRoom(newx, newy, roomYLen, roomXLen);
                        break;
                    case( 2 ):
                        // Moved South
                        newy = newy - prevYSize /2;
                        makeRoom(newx, newy, roomYLen, roomXLen);
                        break;
                    case( 3 ):
                        // Moved West
                        newx = newx - prevXSize /2;
                        makeRoom(newx, newy, roomYLen, roomXLen);
                        break;
                }
                
                // increase the amount of rooms made
                featuresMade++;
            }
            
            
            
            
            
            



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
        LoadTiles();
    }
    
}


