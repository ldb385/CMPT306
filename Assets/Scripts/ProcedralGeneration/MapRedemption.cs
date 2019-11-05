﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class MapRedemption : MonoBehaviour
{
    // map sprites
    [SerializeField] private GameObject Floor;
    [SerializeField] private GameObject northWall;
    [SerializeField] private GameObject eastWall;
    [SerializeField] private GameObject southWall;
    [SerializeField] private GameObject westWall;
    
    // Spawner to add to rooms
    [SerializeField] private Spawner spawner;

    // Set how many rooms to create
    public int roomAmount = 10;
    // Set the Max Length and Width of room and length of Cooridor
    [SerializeField] private int maxRoomWidth = 11;
    [SerializeField] private int maxRoomHeight = 11;
    [SerializeField] private int maxCooridorLength = 8;
    
    

    // create room bool
    private bool createRoom = false;

    // (Phenotype)
    // Visual representation of data
    private Dictionary<Vector2Int, GameObject> tiles;

    // (Genotype) 
    // Representation of coordinates mapped to data
    private Dictionary<Vector2Int, int> data;

    // this is just to set values to unload later
    private int floorG = 0;
    private int wallNorthG = 1;
    private int wallEastG = 2;
    private int wallSouthG = 3;
    private int wallWestG = 4;

    private readonly Action<string> _logger;

    // which direction to move in
    // north: 0, East: 1, South: 2, West: 3


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
                for (tempy = y; tempy < (y + len); tempy++)
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
                for (tempy = y; tempy > (y - len); tempy--)
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
    
    // Remove the Spawners
    private void RemoveSpawners()
    {
        // Destroy all instances of spawners 
        foreach (GameObject obj in Object.FindObjectsOfType<GameObject>()) {
            if( obj.transform.name == "Spawner(Clone)" ){
                Destroy(obj);
            }
        }
    }


    private void UnloadData()
    {
        data = new Dictionary<Vector2Int, int>();
    }

    
    // Despawns the level and then spawns everything based on the most recent data model
    private void LoadTiles()
    {
        UnloadTiles();

        foreach(Vector2Int i in data.Keys)
        {
            GameObject tile;
            if (data[i] == floorG)
            {
                // spawn floor tile at this location
                tile = Instantiate(Floor, (Vector2) i, Quaternion.identity) as GameObject;
                if (!tiles.ContainsKey(i))
                    tiles.Add(i, tile);
            }
            else if (data[i] == wallNorthG )                                                                
            {                                                                                      
                // spawn North wall at this location                                              
                tile = Instantiate( northWall, (Vector2) i, Quaternion.identity) as GameObject;        
                if (!tiles.ContainsKey(i))                                                         
                    tiles.Add(i, tile);                                                            
            }     
            else if (data[i] == wallWestG )                                                                       
            {                                                                                                      
                // spawn West wall at this location                                                               
                tile = Instantiate( westWall, (Vector2) i, Quaternion.identity) as GameObject;                    
                if (!tiles.ContainsKey(i))                                                                         
                    tiles.Add(i, tile);                                                                            
            }
            else if (data[i] == wallSouthG )                                                                       
            {                                                                                                      
                // spawn South wall at this location                                                               
                tile = Instantiate( southWall, (Vector2) i, Quaternion.identity) as GameObject;                    
                if (!tiles.ContainsKey(i))                                                                         
                    tiles.Add(i, tile);                                                                            
            }   
            else if (data[i] == wallEastG )                                                                       
            {                                                                                                      
                // spawn North wall at this location                                                               
                tile = Instantiate( eastWall, (Vector2) i, Quaternion.identity) as GameObject;                    
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
        bool roomsBuiltSuccessfully = true;
        
        // room sizes
        int prevXSize = 7;
        int prevYSize = 7;
        int newx = 0;
        int newy = 0;
        
        // Main loop for building other rooms
        for (int attempts = 0; attempts < 1000; attempts++)
        {
            // if the total rooms is reached break
            if ( roomsMade >= roomAmount)
            {
                break;
            }
            
            // whether a cooridor was built
            bool cooridorbuilt = false;
            
            // pick a random wall to build upon
            int direction = Random.Range(0, 4);
            // pick a random cooridor size
            int cooridorLen = Random.Range(3, maxCooridorLength);

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
                int roomYLen = Random.Range(4, maxRoomHeight);
                int roomXLen = Random.Range(4, maxRoomWidth);
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
                        if ( !makeRoom(newx, newy, roomXLen, roomYLen))
                        {
                            roomsBuiltSuccessfully = false;
                        }
                        break;
                    case( 1 ):
                        // Moved East
                        newx = newx + splitNum( prevXSize, true );
                        if ( !makeRoom(newx, newy, roomXLen, roomYLen))
                        {
                            roomsBuiltSuccessfully = false;
                        }
                        break;
                    case( 2 ):
                        // Moved South
                        newy = newy - splitNum( prevYSize, false );
                        if (!makeRoom(newx, newy, roomXLen, roomYLen))
                        {
                            roomsBuiltSuccessfully = false;
                        }
                        break;
                    case( 3 ):
                        // Moved West
                        newx = newx - splitNum( prevXSize, false );
                        if (!makeRoom(newx, newy, roomXLen, roomYLen))
                        {
                            roomsBuiltSuccessfully = false;
                        }
                        break;
                }
                
                // increase the amount of rooms made
                roomsMade++;
            }
        }


        if ( roomsMade < roomAmount - 3 || roomsMade < cooridorsMade || ! roomsBuiltSuccessfully )
        {
            // if the rooms arent good enough rebuild rooms
            RemoveSpawners();
            UnloadTiles();
            UnloadData();
            GenerateModel();
        }
    }


    private void GenerateWalls()
    {
        // this will apply walls to the Genotype before implementing the Phenotype

        foreach (Vector2Int coord in data.Keys.ToList())
        {
            // note these are all floor at this point so dont need key value pair
            // implementing in same loop to avoid "nested walls"

            // this will check if a north wall needs to be spawned
            Vector2Int northCoord = new Vector2Int(coord.x, coord.y + 1);
            if (!isPlaced(northCoord))
            {
                data.Add(northCoord, wallNorthG);
            }

            // this will check if a south wall needs to be spawned
            Vector2Int southCoord = new Vector2Int(coord.x, coord.y - 1);
            if (!isPlaced(southCoord))
            {
                data.Add(southCoord, wallSouthG);
            }
        }
        
        foreach (Vector2Int coord in data.Keys.ToList()) {   
            // this will check if a East wall needs to be spawned                       
            Vector2Int eastCoord = new Vector2Int( coord.x +1, coord.y );             
            if ( !isPlaced( eastCoord ))                                              
            {                                                                         
                data.Add( eastCoord, wallEastG );                                     
            }                                                                         

            // this will check if a west wall needs to be spawned
            Vector2Int westCoord = new Vector2Int( coord.x -1, coord.y );
            if ( !isPlaced( westCoord ))
            {
                data.Add( westCoord, wallWestG );
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
        // establish room structure
        GenerateModel();
        GenerateWalls();
        LoadTiles();
    }
    
}


