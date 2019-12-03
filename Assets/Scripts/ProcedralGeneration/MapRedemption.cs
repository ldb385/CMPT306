using System;
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
    [SerializeField] private GameObject eastOuterWall;
    [SerializeField] private GameObject westOuterWall;
    [SerializeField] private GameObject eastInnerWall;
    [SerializeField] private GameObject westInnerWall;
    
    // Spawner to add to rooms
    [SerializeField] private Spawner spawner;
    // Doors to add to rooms
    [SerializeField] private GameObject horizontalDoor;
    [SerializeField] private GameObject verticalDoor;
    // Boss scene transition for room
    [SerializeField] private GameObject tele;

    // Set how many rooms to create
    public int roomAmount = 21;
    // Set the Max Length and Width of room and length of Cooridor
    [SerializeField] private int maxRoomWidth = 15;
    [SerializeField] private int maxRoomHeight = 15;
    [SerializeField] private int minRoomWidth = 6;
    [SerializeField] private int minRoomHeight = 6;
    [SerializeField] private int maxCooridorLength = 15;
    
    
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
    private int eastOuterWallG = 5;
    private int westOuterWallG = 6;
    private int eastInnerWallG = 7;
    private int westInnerWallG = 8;

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

        if (!validSpace(x, y, xlen, ylen))
        {
            return false;
        }

        // Check if there is space in Genotype to make room first
        ArrayList coordinates = getRoomCoords(x, y, xlen, ylen);
        foreach (Vector2Int cXY in coordinates)
        {
            if ( isPlaced( cXY ) )
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
            spawner.createSpawner(xlen -1, ylen -1);
            Instantiate(spawner, new Vector3(x - xlen / 2, y - ylen / 2), Quaternion.identity );
        }

        // made it to end must be true
        return true;
    }

    private bool validSpace(int x, int y, int xLength, int yLength)
    {
        // this will be used to make sure that room walls will not overlap 
        int xL = splitNum( xLength, true ) +2;
        int xR = splitNum( xLength, false ) +2;
        int yL = splitNum( yLength, true ) +2;
        int yR = splitNum( yLength, false ) +2;

        // Now fill in coordinate array
        int tempyL;
        int tempyR;
        while (xR > 0)
        {
            tempyL = yL;
            tempyR = yR;
            while (tempyR > 0)
            {
                if (data.ContainsKey(new Vector2Int(x + xR, y + tempyR)))
                {
                    return false;
                }
                tempyR--;
            }

            while (tempyL > 0)
            {
                if (data.ContainsKey(new Vector2Int(x + xR, y - tempyL)))
                {
                    return false;
                }
                tempyL--;
            }

            xR--;
        }

        while (xL > 0)
        {
            tempyL = yL;
            tempyR = yR;
            while (tempyR > 0)
            {
                if (data.ContainsKey(new Vector2Int(x - xL, y + tempyR)))
                {
                    return false;
                }
                tempyR--;
            }
            
            while (tempyL > 0)
            {
                if (data.ContainsKey(new Vector2Int(x - xL, y - tempyL)))
                {
                    return false;
                }
                tempyL--;
            }

            xL--;
        }

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


    // this will be used by make cooridor in order to spawn in doors at the 
    // beginning and end of each cooridor
    private void makeDoor(int x, int y, bool isHorizontal)
    {
        if ( isHorizontal )
        {
            Instantiate( horizontalDoor, new Vector3( x, y ), Quaternion.identity );
        }
        else
        {
            Instantiate( verticalDoor, new Vector3( x, y ), Quaternion.identity );
        }
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
                makeDoor( tempx, y, true );
                for (tempy = y; tempy < (y + len); tempy++)
                {
                    Vector2Int pos = new Vector2Int(tempx, tempy);
                    if (!isPlaced(pos))
                    {
                        data.Add(pos, floorG);
                    }
                }
                makeDoor( tempx, tempy -1, true );
                break;
            case( 1 ): // East
                tempy = y;
                makeDoor( x, tempy, false );
                for (tempx = x; tempx < ( x + len); tempx++)
                {
                    Vector2Int pos = new Vector2Int(tempx, tempy);
                    if (!isPlaced(pos))
                    {
                        data.Add(pos, floorG);
                    }
                }
                makeDoor( tempx -1, tempy, false );
                break;
            case( 2 ): // South
                tempx = x;
                makeDoor( tempx, y, true );
                for (tempy = y; tempy > (y - len); tempy--)
                {
                    Vector2Int pos = new Vector2Int(tempx, tempy);
                    if (!isPlaced(pos))
                    {
                        data.Add(pos, floorG);
                    }
                }
                makeDoor( tempx, tempy +1, true );
                break;
            case( 3 ): // West
                tempy = y;
                makeDoor( x -1, tempy, false );
                for (tempx = x; tempx > (x - len); tempx--)
                {
                    Vector2Int pos = new Vector2Int(tempx, tempy);
                    if (!isPlaced(pos))
                    {
                        data.Add(pos, floorG);
                    }
                }
                makeDoor( tempx +1, tempy, false );
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
    
    // Remove the Spawners and the Doors
    private void RemoveExcess()
    {
        // Destroy all instances of spawners and doors
        foreach (GameObject obj in Object.FindObjectsOfType<GameObject>()) {
            if( obj.transform.name == "Spawner(Clone)" ){
                Destroy(obj);
            }
            if( obj.transform.name == "SpawnerLevelOne(Clone)" ){
                Destroy(obj);
            }
            if( obj.transform.name == "SpawnerLevelTwo(Clone)" ){
                Destroy(obj);
            }
            if( obj.transform.name == "SpawnerLevelThree(Clone)" ){
                Destroy(obj);
            }
            if( obj.transform.name == "VerDoor(Clone)" ){
                Destroy(obj);
            }
            if( obj.transform.name == "HorDoor(Clone)" ){
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
                // spawn East wall at this location                                                               
                tile = Instantiate( eastWall, (Vector2) i, Quaternion.identity) as GameObject;                    
                if (!tiles.ContainsKey(i))                                                                         
                    tiles.Add(i, tile);                                                                            
            }     
            else if (data[i] == eastOuterWallG )                                                                       
            {                                                                                                      
                // spawn East outer wall at this location                                                               
                tile = Instantiate( eastOuterWall, (Vector2) i, Quaternion.identity) as GameObject;                    
                if (!tiles.ContainsKey(i))                                                                         
                    tiles.Add(i, tile);                                                                            
            }   
            else if (data[i] == westOuterWallG )                                                                       
            {                                                                                                      
                // spawn West outer wall at this location                                                               
                tile = Instantiate( westOuterWall, (Vector2) i, Quaternion.identity) as GameObject;                    
                if (!tiles.ContainsKey(i))                                                                         
                    tiles.Add(i, tile);                                                                            
            }   
            else if (data[i] == eastInnerWallG )                                                                       
            {                                                                                                      
                // spawn east inner wall at this location                                                               
                tile = Instantiate( eastInnerWall, (Vector2) i, Quaternion.identity) as GameObject;                    
                if (!tiles.ContainsKey(i))                                                                         
                    tiles.Add(i, tile);                                                                            
            }   
            else if (data[i] == westInnerWallG )                                                                       
            {                                                                                                      
                // spawn west inner wall at this location                                                               
                tile = Instantiate( westInnerWall, (Vector2) i, Quaternion.identity) as GameObject;                    
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
                int roomYLen = Random.Range(minRoomHeight, maxRoomHeight);
                int roomXLen = Random.Range(minRoomWidth, maxRoomWidth);
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

                bool spawnEnemiesInRoom = (roomsMade + 1 < roomAmount);
                
                switch( direction )
                {
                    case( 0 ):
                        // Moved North
                        newy = newy + splitNum( prevYSize, true );
                        if ( !makeRoom(newx, newy, roomXLen, roomYLen, spawnEnemiesInRoom))
                        {
                            roomsBuiltSuccessfully = false;
                        }
                        break;
                    case( 1 ):
                        // Moved East
                        newx = newx + splitNum( prevXSize, true );
                        if ( !makeRoom(newx, newy, roomXLen, roomYLen, spawnEnemiesInRoom))
                        {
                            roomsBuiltSuccessfully = false;
                        }
                        break;
                    case( 2 ):
                        // Moved South
                        newy = newy - splitNum( prevYSize, false );
                        if (!makeRoom(newx, newy, roomXLen, roomYLen, spawnEnemiesInRoom))
                        {
                            roomsBuiltSuccessfully = false;
                        }
                        break;
                    case( 3 ):
                        // Moved West
                        newx = newx - splitNum( prevXSize, false );
                        if (!makeRoom(newx, newy, roomXLen, roomYLen, spawnEnemiesInRoom))
                        {
                            roomsBuiltSuccessfully = false;
                        }
                        break;
                }
                
                // increase the amount of rooms made
                roomsMade++;
            }
        }


        if ( roomsMade < roomAmount || roomsMade < cooridorsMade || ! roomsBuiltSuccessfully )
        {
            // if the rooms arent good enough rebuild rooms
            RemoveExcess();
            UnloadTiles();
            UnloadData();
            GenerateModel();
        }
        else
        {
            // made it to last room can now place the teleporter to the boss
            Instantiate( tele, new Vector3( newx, newy ), Quaternion.identity );
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
        
        // this will check the east and west wall
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
        
        // apply Inner and Outer walls with bricks
        foreach (Vector2Int coord in data.Keys.ToList()) {   
            // this will check if a Inner wall must be spawned                     
            if (data[ coord ] == floorG &&
                data[new Vector2Int(coord.x +1, coord.y)] == wallSouthG &&
                data[new Vector2Int(coord.x -1, coord.y)] == wallSouthG)
            {
                // check south path
                data[new Vector2Int(coord.x +1, coord.y)] = eastInnerWallG;
                data[new Vector2Int(coord.x -1, coord.y)] = westInnerWallG;
            }
            if (data[ coord ] == floorG &&
                     data[new Vector2Int(coord.x, coord.y +1)] == wallNorthG && 
                     data[new Vector2Int(coord.x, coord.y -1)] == wallSouthG )
            {
                // check east path
                if( data.ContainsKey( new Vector2Int(coord.x, coord.y +2 )) &&
                    data.ContainsKey( new Vector2Int(coord.x, coord.y -2 )) &&
                    data[new Vector2Int(coord.x, coord.y +2 )] == wallEastG &&
                    data[new Vector2Int(coord.x, coord.y -2 )] == wallEastG )
                {
                    // make sure it is in dict before trying to index
                    data[new Vector2Int(coord.x, coord.y -1)] = eastInnerWallG;
                }
            }
            if (data[ coord ] == floorG &&
                     data[new Vector2Int(coord.x, coord.y +1)] == wallNorthG && 
                     data[new Vector2Int(coord.x, coord.y -1)] == wallSouthG )
            {
                // check west path
                if( data.ContainsKey( new Vector2Int(coord.x, coord.y +2 )) &&
                    data.ContainsKey( new Vector2Int(coord.x, coord.y -2 )) &&
                    data[new Vector2Int(coord.x, coord.y +2 )] == wallWestG &&
                    data[new Vector2Int(coord.x, coord.y -2 )] == wallWestG )
                {
                    // make sure it is in dict before trying to index
                    data[new Vector2Int(coord.x, coord.y -1)] = westInnerWallG;
                }
            }
            
            // this will check if a Outer wall must be spawned
            if (data[coord] == wallWestG &&
                     data[new Vector2Int( coord.x +1, coord.y ) ] == wallSouthG &&
                     !isPlaced(new Vector2Int(coord.x -1, coord.y)))
            {
                // check west path
                data[coord] = westOuterWallG;
            }
            if (data[coord] == wallEastG &&
                     data[new Vector2Int( coord.x -1, coord.y ) ] == wallSouthG &&
                     !isPlaced(new Vector2Int(coord.x +1, coord.y)))
            {
                // check east path
                data[coord] = eastOuterWallG;
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


