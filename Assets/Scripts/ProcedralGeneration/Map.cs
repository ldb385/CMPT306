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

    // map size
    public int mWidth = 100;
    public int mHeight = 100;


    // choice weights
    private int upW = 10;
    private int downW = 10;
    private int leftW = 10;
    private int rightW = 10; 
    private int roomW = 5;
    // amount of rooms
    private int curRoom = 0;


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
        
        xPos = mWidth/2;
        yPos = mHeight/2;
        
        while (curRoom <= RoomCount){
            Vector2Int coord = (CalculateChoice(xPos, yPos, tmp_model));
            if(!tmp_model.ContainsKey(coord)){
                    tmp_model.Add(coord, 1);
                }
        }
        return tmp_model;
    }

        private void RoomCreation(int xPos, int yPos, Dictionary<Vector2Int, int> tmp_model){
            RoomWidth = Random.Range(minWidth, maxWidth);
            RoomHeight = Random.Range(minHeight, maxHeight);
            
            // for(int i=yPos; i<RoomWidth; i++){
            //     for(int j=xPos; j<RoomHeight; j++){
            //         Vector2Int roomCoord = CalculateChoice(i, j, tmp_model);
            //         if(!tmp_model.ContainsKey(roomCoord)){
            //             tmp_model.Add(roomCoord, 0);
            //         }
            //     }
            // }
        }


    public Vector2Int CalculateChoice(int x, int y, Dictionary<Vector2Int, int> tmp_model){
    
    int weightTotal = upW+downW+leftW+rightW+roomW;
    // THIS IS BROKEN NEED TO FIX THIS PIECE OF SHIT
     int RandomChoice = Random.Range(0, weightTotal);
     if ((RandomChoice -= upW) <= 0){
        upW+=10;
        downW+=9;
        leftW+=9;
        rightW+=9;
        roomW+=4;
        yPos+=1;
        return new Vector2Int(x, y+1);
     }
    else if ((RandomChoice -= downW) <= 0){
        upW+=10;
        downW+=10;
        leftW+=10;
        rightW+=10;
        roomW+=4;
        Debug.Log(2);
        yPos-=1;
        return new Vector2Int(x, y-1);
     }
    else if ((RandomChoice -= leftW) <= 0){
        upW+=10;
        downW+=10;
        leftW+=10;
        rightW+=10;
        roomW+=4;
        Debug.Log(3);
        xPos-=1;
        return new Vector2Int(x-1, y);
    }
    else if ((RandomChoice -= rightW) <= 0){
        upW+=10;        
        downW+=10;
        leftW+=10;
        rightW+=10;
        roomW+=4;
        Debug.Log(4);
        xPos+=1;
        return new Vector2Int(x+1, y);
    }
    else if ((RandomChoice -= roomW) <= 0){
        // create room
        RoomCreation(xPos,yPos, tmp_model);
        curRoom= curRoom + 1;
        upW=0;
        downW=0;
        leftW=0;
        rightW=0;
        roomW=0;
        Debug.Log(5);
        return new Vector2Int(x, y);
    }
    
    return new Vector2Int(0, 0);
 }


    // Allow quick reload of level for testing
    // This should be removed in a real game context
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            data = GenerateModel();
            LoadTiles();
        }
    }
}


