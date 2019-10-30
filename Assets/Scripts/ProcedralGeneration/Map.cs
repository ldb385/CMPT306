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

    // min max room size  
    public int maxWidth=0;
    public int minWidth=0;

    public int maxHeight=0;
    public int minHeight=0;

    private int RoomWidth;
    private int RoomHeight;

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

    // REPLACE THIS CODE WITH YOUR GENERATION ALGORITHM
    public Dictionary<Vector2Int, int> GenerateModel(){

        Dictionary<Vector2Int, int> tmp_model = new Dictionary<Vector2Int, int>();

        int Mapwidth = 100;
        int Mapheight = 100;

        for(int i=0; i<width; i++)
        {
            for(int j=0; j<height; j++)
            {
                Vector2Int coord = new Vector2Int(i, j);

                int rnd = Random.Range(0, 2);
                if(!tmp_model.ContainsKey(coord))
                {
                    tmp_model.Add(coord, rnd);
                }
            }
        }
        return tmp_model;
    }

    private void CreateRoom(){
        // Random random = new Random();
        RoomWidth = Random.Range(maxWidth, minWidth);
        RoomHeight = Random.Range(minHeight, maxHeight);
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
