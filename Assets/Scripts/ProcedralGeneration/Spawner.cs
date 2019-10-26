using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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
    [SerializeField] private GameObject table;
    [SerializeField] private GameObject chairSide;
    [SerializeField] private GameObject chairBack;


    // This will store the game tiles (Phenotype)
    private Dictionary<Vector2Int, GameObject> tiles;
    // This will store the underlying data structure (Genotype)
    private Dictionary<Vector2Int, int> data;
    

    // the dimension of the room
    public int width;
    public int height;
    
    // Used for spook level and Player spook
    public int spookPercent;
    private float spookLevel;
    private int minEnemies = 3;
    
    private void Awake()
    {
        // Create the data structures <VERY IMPORTANT THIS IS DONE FIRST>
        tiles = new Dictionary<Vector2Int, GameObject>();
        data = new Dictionary<Vector2Int, int>();

        data = GenerateModel();
        
        LoadTiles();
    }

    private void Start()
    {
        spookLevel = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().spookLevel;
        
        data = GenerateModel();
        LoadTiles();
    }

    private GameObject getObstacle()
    {
        int rnd = Random.Range(0, 4);

        switch (rnd)
        {
            case 0:
                // Spawn in stool
                return endTable;
            case 1:
                return table;
            case 2:
                return chairSide;
            case 3:
                return chairBack;
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
        Vector2 diff = new Vector2(transform.position.x, transform.position.y);

        foreach( Vector2Int i in data.Keys )
        {
            // 0 Empty, 1 Obstacle, 2 Enemy
            switch( data[i] ){
                case 1:
                    // Spawn in Obstacle
                    tile = Instantiate( getObstacle(), (Vector2)i + diff, Quaternion.identity) as GameObject;
                    break;
                case 2:
                    // Spawn in Enemy
                    tile = Instantiate( getEnemy( (int) spookLevel * ( spookPercent/100) ) ,
                        (Vector2)i + diff, Quaternion.identity) as GameObject;
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
    


    public Dictionary<Vector2Int, int> GenerateModel()
    {

        int enemies = ( spookPercent / 100 ) * (int) ( 10.0f - spookLevel );

        if (enemies < minEnemies)
        {
            enemies = minEnemies;
        }

        int avelen = width + height / 2;
        int obstacles = ( avelen * 3) / Random.Range( avelen/2, avelen) ;
        
        Dictionary<Vector2Int, int> tmp_model = new Dictionary<Vector2Int, int>();

        int spawned = Random.Range(1, 5);
        
        for(int i=0; i<width; i++)
        {
            for(int j=0; j<height; j++)
            {
                Vector2Int coord = new Vector2Int(i, j);

                spawned = spawned - Random.Range(0, 3);

                if (spawned <= 0)
                {
                    // Pick whether its empty, obstacle, enemy
                    int rnd = Random.Range(0, avelen );
                    switch (rnd)
                    {
                        // 0 Empty, 1 Obstacle, 2 Enemey
                        case 1:
                            obstacles--;
                            if (obstacles <= 0)
                            {
                                rnd = 0;
                                spawned = Random.Range(0, 5);
                            }

                            break;
                        case 2:
                            if (enemies <= 0)
                            {
                                rnd = 0;
                                spawned = Random.Range(0, 5);
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
