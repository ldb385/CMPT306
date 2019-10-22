using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // ALL ENEMIES
    [SerializeField] private GameObject Ghost;
    [SerializeField] private GameObject Skeleton;
    [SerializeField] private GameObject Mummy;
    [SerializeField] private GameObject Zombie;
    [SerializeField] private GameObject Alien;
    
    // ALL OBSTACLES
    [SerializeField] private GameObject endTable;

    // This will store the game tiles (Phenotype)
    private Dictionary<Vector2Int, GameObject> tiles;
    // This will store the underlying data structure (Genotype)
    private Dictionary<Vector2Int, int> data;
    

    // the dimension of the room
    public int width;
    public int height;
    
    // Used for testing
    public int spookMult;
    
    private void Awake()
    {
        // Create the data structures <VERY IMPORTANT THIS IS DONE FIRST>
        tiles = new Dictionary<Vector2Int, GameObject>();
        data = new Dictionary<Vector2Int, int>();

        data = GenerateModel();
        LoadTiles();
    }

    private GameObject getObstacle()
    {
        int rnd = Random.Range(0, 1);

        switch (rnd)
        {
            case 0:
                // Spawn in stool
                return endTable;
        }

        // It will never hit this
        return endTable;
    }


    private GameObject getEnemy( int max )
    {
        if (max > 5)
        {
            max = 5;
        }

        int rnd = Random.Range(0, max);

        switch (rnd)
        {
            case 0:
                // Spawn in stool
                return Skeleton;
            case 1:
                return Ghost;
            case 2:
                return Zombie;
            case 3:
                return Mummy;
            case 4:
                return Alien;
        }

        // It will never hit this
        return Skeleton;
    }
    
    
    /**
     * Despawns the level and then spawns everything based on the most recent data model
     */
    private void LoadTiles()
    {
        // Despawn the tiles
        UnloadTiles();

        GameObject tile;

        foreach( Vector2Int i in data.Keys )
        {
            // 0 Empty, 1 Obstacle, 2 Enemy
            switch( data[i] ){
                case 1:
                    
                    // Spawn in Obstacle
                    tile = Instantiate( getObstacle(), (Vector2)i, Quaternion.identity) as GameObject;
                    break;
                case 2:
                    // Spawn in Enemy
                    tile = Instantiate( getEnemy( 2 ) , (Vector2)i, Quaternion.identity) as GameObject;
                    break;
                default:
                    tile = null;
                    break;
            }
            // Check to make sure it doesn't duplicate
            if (!tiles.ContainsKey(i))
                tiles.Add(i, tile);
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

    // If model gen is happening in a different script or GameObject, use this to set the model
    public void SetModel(Dictionary<Vector2Int, int> model)
    {
        data = model;
    }

    // Generate a new model for the level
    // Currently random 10X10
    // REPLACE THIS CODE WITH YOUR GENERATION ALGORITHM
    public Dictionary<Vector2Int, int> GenerateModel()
    {
//        float spook = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().spookLevel;

        // WILL BE REMOVED
        int enemies = ( int ) spookMult;
        int obstacles = 5;
        
        Dictionary<Vector2Int, int> tmp_model = new Dictionary<Vector2Int, int>();

        for(int i=0; i<width; i++)
        {
            for(int j=0; j<height; j++)
            {
                Vector2Int coord = new Vector2Int(i, j);
                
                // Pick whether its empty, obstacle, enemy
                int rnd = Random.Range( 0, 3 );
                switch (rnd)
                {
                    // 0 Empty, 1 Obstacle, 2 Enemey
                    case 1:
                        obstacles--;
                        if ( obstacles <= 0)
                        {
                            rnd = 0;
                        }
                        break;
                    case 2:
                        if ( enemies <= 0)
                        {
                            rnd = 0;
                        }
                        enemies--;
                        break;
                }
                
                // Check to make sure the coordinate isn't already added to dict
                if(!tmp_model.ContainsKey(coord))
                {
                    tmp_model.Add(coord, rnd);
                }
            }
        }
        // Return the current model
        return tmp_model;
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
