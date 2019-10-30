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
    public int RoomCount=10;

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
    private int upW = 0;
    private int downW = 0;
    private int leftW = 0;
    private int rightW = 0; 
    private int roomW = 0;
    // amount of rooms
    private int curRoom = 0;


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
        
        
        int xPos = mWidth/2;
        int yPos = mHeight/2;
        
        
        while (curRoom <= RoomCount){
            Vector2Int coord = (CalculateChoice(xPos, yPos, tmp_model));
            if(!tmp_model.ContainsKey(coord)){
                // {       // coord, rnd=tile
                    tmp_model.Add(coord, 0);
                }
        }
        return tmp_model;
    }


        // for(int i=0; i<mWidth; i++)
        // {
        //     for(int j=0; j<mHeight; j++)
        //     {
                
        //         Vector2Int coord = new Vector2Int(i, j);

                 

        //         if(!tmp_model.ContainsKey(coord))
        //         {       // coord, rnd=tile
        //             // tmp_model.Add(coord, rnd);
        //         }
        //     }
        // }
        private void RoomCreation(int xPos, int yPos, Dictionary<Vector2Int, int> tmp_model){
            for(int i=yPos; i<RoomWidth; i++){
                for(int j=xPos; j<RoomHeight; j++){
                    Vector2Int roomCoord = CalculateChoice(i, j, tmp_model);
                    if(!tmp_model.ContainsKey(roomCoord)){
                        tmp_model.Add(roomCoord, 0);
                    }
                }
            }
        }


    public Vector2Int CalculateChoice(int xPos, int yPos, Dictionary<Vector2Int, int> tmp_model){
    






    
    // float weightTotal = upW+downW+leftW+rightW+roomW;
    // // THIS IS BROKEN NEED TO FIX THIS PIECE OF SHIT
    //  int RandomChoice = Random.Range(0, (int)weightTotal);
    //  if ((RandomChoice -= upW) < 0){
    //     upW=0;
    //     downW+=10;
    //     leftW+=10;
    //     rightW+=10;
    //     roomW+=10;
    //     return new Vector2Int(xPos, yPos+1);
    //  }
    // else if ((RandomChoice -= downW) < 0){
    //     upW+=10;
    //     downW=0;
    //     leftW+=10;
    //     rightW+=10;
    //     roomW+=10;
    //     return new Vector2Int(xPos, yPos-1);
    //  }
    // else if ((RandomChoice -= leftW) < 0){
    //     upW+=10;
    //     downW+=10;
    //     leftW=10;
    //     rightW+=10;
    //     roomW+=10;
    //     return new Vector2Int(xPos-1, yPos);
    // }
    // else if ((RandomChoice -= rightW) < 0){
    //     upW+=10;        
    //     downW+=10;
    //     leftW+=10;
    //     rightW=0;
    //     roomW+=10;
    //     return new Vector2Int(xPos+1, yPos);
    // }
    // else if ((RandomChoice -= roomW) < 0){
    //     // create room
    //     RoomCreation(xPos,yPos, tmp_model);
    //     curRoom= curRoom + 1;
    //     upW+=10;
    //     downW+=10;
    //     leftW+=10;
    //     rightW+=10;
    //     roomW=0;
    //     return new Vector2Int(xPos, yPos);
    // }
    // return new Vector2Int(xPos, yPos);
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


