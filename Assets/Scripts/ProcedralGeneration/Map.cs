using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    // map sprites
    [SerializeField] 
    private GameObject Floor;
    [SerializeField] 
    private GameObject Wall;

    // (Phenotype)
    private Dictionary<Vector2Int, GameObject> tiles;
    // (Genotype)
    private Dictionary<Vector2Int, int> data;

    // amount of rooms
    public int RoomCount=100;

    // min max room width  
    public int maxWidth=10;
    public int minWidth=0;
    
    // min max room height
    public int maxHeight=10;
    public int minHeight=0;

    // room width height
    private int RoomWidth;
    private int RoomHeight;

    // choice weights
    private int upW = 100;
    private int downW = 100;
    private int leftW = 100;
    private int rightW = 100; 
    private int roomW = 0;
    
    // amount of rooms
    private int curRoom = 0;
    
    // create room bool
    private bool createRoom = false;
    
    // global xy position
    private int xPos;
    private int yPos;


    private void Awake()
    {
        // Create the data structures <VERY IMPORTANT THIS IS DONE FIRST>
        tiles = new Dictionary<Vector2Int, GameObject>();
        data = new Dictionary<Vector2Int, int>();

        data = GenerateModel();
        LoadTiles();
    }

    // Despawns the level and then spawns everything based on the most recent data model
    private void LoadTiles()
    {
        UnloadTiles();

        foreach(Vector2Int i in data.Keys)
        {
            GameObject tile;
            if(data[i] == 0)
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

// genotype creation     
    public Dictionary<Vector2Int, int> GenerateModel(){
        // initializing the temporary model
        Dictionary<Vector2Int, int> tmp_model = new Dictionary<Vector2Int, int>();
        
        // starting position
        xPos = 0;
        yPos = 0;
        
        // while not all rooms are made
        while (curRoom <= RoomCount){
            // get what direction to move
            Vector2Int coord = (CalculateChoice(xPos, yPos, tmp_model));
            
            // if we are making a room
            if( createRoom == false){
                // not making a room
                if(!tmp_model.ContainsKey(coord)){
                        tmp_model.Add(coord, 0);
                }
            }
            else{
                // we are building a room
                // THIS PART IS NOT RIGHT

                RoomWidth = Random.Range(minWidth, maxWidth);
                RoomHeight = Random.Range(minHeight, maxHeight);
                // increment the amount of rooms by one
                curRoom= curRoom + 1;

                /* TODO:
                    replace this with an array of preset vecters?
                    or
                    find the reason why the two stupid arrays dont work
                    */ 
                    
                // get positions of where the room is going to be
                for(int i=yPos; i<=RoomHeight; i++){
                    for(int j=xPos; j<=RoomWidth; j++){
                        Vector2Int roomCoord = new Vector2Int(i,j);
                        tmp_model.Remove(roomCoord);
                        if(!tmp_model.ContainsKey(roomCoord)){
                            tmp_model.Add(roomCoord, 1);
                            // Debug.Log(roomCoord);
                        }
                    }
                }
            createRoom = false;
            }
        }
        return tmp_model;
    }


    public Vector2Int CalculateChoice(int x, int y, Dictionary<Vector2Int, int> tmp_model){

        // total weight
    int weightTotal = upW+downW+leftW+rightW+roomW;
        // get the choice 
     int RandomChoice = Random.Range(0, weightTotal);
        // move up
     if ((RandomChoice -= upW) <= 0){
        upW+=10;
        downW+=2;
        leftW+=2;
        rightW+=2;
        roomW+=2;
        yPos+=1;
        Debug.Log(1);
        return new Vector2Int(x, y+1);
     }
        // move down
    else if ((RandomChoice -= downW) <= 0){
        upW+=2;
        downW+=10;
        leftW+=2;
        rightW+=2;
        roomW+=7;
        yPos-=1;
        Debug.Log(1);
        return new Vector2Int(x, y-1);
     }
        // move left
    else if ((RandomChoice -= leftW) <= 0){
        upW+=2;
        downW+=2;
        leftW+=10;
        rightW+=2;
        roomW+=1;
        xPos-=1;
        Debug.Log(1);
        return new Vector2Int(x-1, y);
    }
        // move right
    else if ((RandomChoice -= rightW) <= 0){
        upW+=2;        
        downW+=2;
        leftW+=2;
        rightW+=5;
        roomW+=10;
        xPos+=1;
        Debug.Log(1);

        return new Vector2Int(x+1, y);
    }
        // create room
    else if ((RandomChoice -= roomW) <= 0){
        // create room
        // RoomCreation(xPos,yPos, tmp_model);
        curRoom= curRoom + 1;
        upW=1;
        downW=1;
        leftW=2;
        rightW=2;
        roomW=0;
        createRoom = true;
        Debug.Log(curRoom);

        return new Vector2Int(xPos, yPos);
    }
    
    return new Vector2Int(xPos, yPos);
}

    // Allow quick reload of level for testing
    // This should be removed in a real game context
    private void Update()
    {
        // if(Input.GetKeyDown(KeyCode.R))
        // {
            // data = GenerateModel();
            // LoadTiles();
        // }
    }
}


