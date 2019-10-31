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
    private int upW = 10;
    private int downW = 10;
    private int leftW = 10;
    private int rightW = 10; 
    private int roomW = 0;
    // amount of rooms
    private int curRoom = 0;

    private bool createRoom = false;



    // private int Stra = 1;
    // private int turn = 0;


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

        Dictionary<Vector2Int, int> tmp_model = new Dictionary<Vector2Int, int>();
        
        xPos = 0;
        yPos = 0;
        
        while (curRoom <= RoomCount){
            Vector2Int coord = (CalculateChoice(xPos, yPos, tmp_model));
            
            if( createRoom == false){
                if(!tmp_model.ContainsKey(coord)){
                        tmp_model.Add(coord, 0);
                }
            }
            else{
                RoomWidth = Random.Range(minWidth, maxWidth);
                RoomHeight = Random.Range(minHeight, maxHeight);
                curRoom= curRoom + 1;

                for(int i=yPos; i<RoomWidth; i++){
                    for(int j=xPos; j<RoomHeight; j++){
                        Vector2Int roomCoord = new Vector2Int(j,i);
                        if(tmp_model.ContainsKey(roomCoord)){
                            tmp_model.Remove(roomCoord);
                            tmp_model.Add(roomCoord, 1);
                            Debug.Log(roomCoord);
                        }
                        else{
                            tmp_model.Add(roomCoord, 1);
                        }
                    }
                }
            createRoom = false;
            }
        }
        return tmp_model;
    }


    public Vector2Int CalculateChoice(int x, int y, Dictionary<Vector2Int, int> tmp_model){


    int weightTotal = upW+downW+leftW+rightW+roomW;
    // THIS IS BROKEN NEED TO FIX THIS PIECE OF SHIT
     int RandomChoice = Random.Range(0, weightTotal);
     if ((RandomChoice -= upW) <= 0){
        upW+=10;
        downW+=2;
        leftW+=2;
        rightW+=2;
        roomW+=5;
        yPos+=1;
        return new Vector2Int(x, y+1);
     }
    else if ((RandomChoice -= downW) <= 0){
        upW+=2;
        downW+=10;
        leftW+=2;
        rightW+=2;
        roomW+=2;
        yPos-=1;
        return new Vector2Int(x, y-1);
     }
    else if ((RandomChoice -= leftW) <= 0){
        upW+=2;
        downW+=2;
        leftW+=10;
        rightW+=2;
        roomW+=5;
        xPos-=1;
        return new Vector2Int(x-1, y);
    }
    else if ((RandomChoice -= rightW) <= 0){
        upW+=2;        
        downW+=2;
        leftW+=2;
        rightW+=10;
        roomW+=2;
        xPos+=1;
        return new Vector2Int(x+1, y);
    }
    else if ((RandomChoice -= roomW) <= 0){
        // create room
        // RoomCreation(xPos,yPos, tmp_model);
        curRoom= curRoom + 1;
        upW=0;
        downW=0;
        leftW=0;
        rightW=0;
        roomW=0;
        createRoom = true;
        Debug.Log(1);
        return new Vector2Int(x, y);
    }
    
    return new Vector2Int(0, 0);
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


