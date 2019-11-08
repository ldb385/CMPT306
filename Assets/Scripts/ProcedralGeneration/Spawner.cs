using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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

	// ALL DESPOOKERS
	[SerializeField] private GameObject Flashlight;
	[SerializeField] private GameObject Teddy;
	[SerializeField] private GameObject Candy;

	// ALL POWERUPS
	[SerializeField] private GameObject Nerf;
	[SerializeField] private GameObject Soda;
	[SerializeField] private GameObject Balloon;
	[SerializeField] private GameObject Cape;

    // This will store the game tiles (Phenotype)
    private Dictionary<Vector2Int, GameObject> tiles;
    // This will store the underlying data structure (Genotype)
    private Dictionary<Vector2Int, int> data;
    
    // Whether enemies were already spawned
    private bool hasBeenActivated = false;

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
    }


    private void Start()
    {
        createSpawner( width, height );
    }


    private GameObject getObstacle()
    {
        int rnd = Random.Range(0, 4);

        switch (rnd)
        {
            case 0:
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

	private GameObject getDespooker( int max )
	{
		if (max > 4)
		{
			max = 4;
		}
	
		int rnd = Random.Range(0, max );
		switch(rnd)
		{
			case 0:
				return Candy;
			case 1:
				return Teddy;
			case 2:
				return Teddy;
			case 3:
				return Flashlight;
			case 4:
				return Flashlight;
		}

		// It will never hit THIS
		return Candy;
	}

	private GameObject getPowerUp( int max )
	{
		if (max > 4)
		{
			max = 4;
		}
	
		int rnd = Random.Range(0, max );
		switch(rnd)
		{
			case 0:
				return Soda;
			case 1:
				return Nerf;
			case 2:
				return Balloon;
			case 3:
				return Balloon;
			case 4:
				return Cape;
		}

		// It will never hit THIS
		return Soda;
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
            // 0 Empty, 1 Obstacle, 2 Enemy, 3 Despooker, 4 PowerUp
            switch( data[i] ){
                case 1:

                    // Spawn in Obstacle
                    tile = Instantiate( getObstacle(), (Vector2)i + diff, Quaternion.identity) as GameObject;
                    break;
                case 2:
                    if ( spookLevel <= 0 )
                    {
                        // Spawn in Enemy
                        tile = Instantiate(getEnemy( 9 ),
                            (Vector2) i + diff, Quaternion.identity) as GameObject;
                    }
                    else
                    {
                        // Spawn in Enemy
                        tile = Instantiate(getEnemy((int) (9.0f - spookLevel)),
                            (Vector2) i + diff, Quaternion.identity) as GameObject;
                    }

                    break;
                case 3:
                    if ( spookLevel <= 0 )
                    {
                        // Spawn in Despooker
						tile = Instantiate(getDespooker( 9 ),
							(Vector2) i + diff, Quaternion.identity) as GameObject;
                    }
                    else
                    {
                        // Spawn in Despooker
						tile = Instantiate(getDespooker((int)(9.0f - spookLevel) ),
							(Vector2) i + diff, Quaternion.identity) as GameObject;
                    }

                    break;
				case 4:
                    if ( spookLevel <= 0 )
                    {
                        // Spawn in PowerUp
						tile = Instantiate(getPowerUp( 9 ),
							(Vector2) i + diff, Quaternion.identity) as GameObject;
                    }
                    else
                    {
                        // Spawn in PowerUp
						tile = Instantiate(getPowerUp((int) (9.0f - spookLevel)),
							(Vector2) i + diff, Quaternion.identity) as GameObject;
                    }

                    break;
				default:
                    tile = null;
                    break;
            }
            // Check to make sure it doesn't duplicate
            if ( !tiles.ContainsKey(i) )
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
    


    public Dictionary<Vector2Int, int> GenerateModel( int w, int h )
    {

        int enemies = (int) ( 9.0f - spookLevel );

        if (enemies < minEnemies)
        {
            enemies = minEnemies;
        }

		int powers = (int) ( 9.0f - spookLevel );


		int despooks = (int) ( 9.0f - spookLevel );


        int avelen = w + h / 2;
        int obstacles = ( avelen * 3) / Random.Range( avelen/2, avelen) ;
        int distFromObs = 3;
        int distFromEne = 3;
		int distFromPow = 3;
		int distFromDes = 3;

        Dictionary<Vector2Int, int> tmp_model = new Dictionary<Vector2Int, int>();

        int spawned = Random.Range(1, 5);
        
        for(int i=0; i<w; i++)
        {	// iterate through width
            for(int j=0; j<h; j++)
            {	// iterate through height
                Vector2Int coord = new Vector2Int(i, j);

                //I changed this from 3 to 1 to account for the other cases I think...
                spawned = spawned - 1;

                if (spawned <= 0)
                {
                    // Pick whether its empty, obstacle, enemy
                    int rnd = Random.Range(0, avelen );
                    
                    
                    // need to make sure you are not spawning in front of a potential door
                    if (i == w / 2 && (j == 0 || j == h - 1) ||
                        j == h / 2 && (i == 0 || i == w - 1))
                    {
	                    rnd = 0;
                    }
                    else
                    {
	                    switch (rnd)
	                    {
		                    // 0 Empty, 1 Obstacle, 2 Enemey
		                    case 1:
			                    obstacles--;
			                    if (obstacles <= 0 || distFromObs < 2)
			                    {
				                    // if there is too many or too close
				                    rnd = 0;
				                    spawned = Random.Range(0, avelen);
				                    distFromObs++;
			                    }
			                    else
			                    {
				                    distFromObs = 0;
			                    }


			                    break;
		                    case 2:
			                    if (enemies <= 0 || distFromEne < 2)
			                    {
				                    // if there is too many or too close
				                    rnd = 0;
				                    spawned = Random.Range(0, avelen);
				                    distFromEne++;
			                    }
			                    else
			                    {
				                    distFromEne = 0;
			                    }

			                    enemies--;
			                    break;
		                    case 3:
			                    if (powers <= 0 || distFromPow < 2)
			                    {
				                    // if there is too many or too close
				                    rnd = 0;
				                    spawned = Random.Range(0, avelen);
				                    distFromPow++;
			                    }
			                    else
			                    {
				                    distFromPow = 0;
			                    }

			                    break;
		                    case 4:
			                    if (despooks <= 0 || distFromDes < 2)
			                    {
				                    // if there is too many or too close
				                    rnd = 0;
				                    spawned = Random.Range(0, avelen);
				                    distFromDes++;
			                    }
			                    else
			                    {
				                    distFromDes = 0;
			                    }

			                    break;
	                    }
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


    public void createSpawner( int wdt, int hgt )
    {
        // Create a rectangle that encapsulates the room so that we can use it as a trigger
        // this is for visual purposes
        //GameObject trig = GameObject.CreatePrimitive(PrimitiveType.Cube );
        //trig.transform.position = new Vector2( transform.position.x + wdt/2, transform.position.y + hgt/2);
        //trig.transform.localScale =  new Vector3(wdt, hgt, 1);
        //trig.GetComponent<Renderer>().enabled = false;
        //trig.GetComponent<Collider>().isTrigger = true;
        //trigger = trig;

        width = wdt;
        height = hgt;
    }

    public void spawnIn( )
    {
        data = GenerateModel( width, height );
        LoadTiles();
        hasBeenActivated = true;

    }


    // Allow quick reload of level for testing
    // This should be removed in a real game context
    private void Update()
    {
        Vector2 player = GameObject.FindGameObjectWithTag("Player").transform.position;
        GameObject wiz = GameObject.FindGameObjectWithTag("Player");
        Player playerScript = wiz.GetComponent<Player>();
        spookLevel = playerScript.spookLevel;
        if (player.x > transform.position.x && player.x < transform.position.x + width
                                            && player.y > transform.position.y &&
                                            player.y < transform.position.y + height &&
                                            !hasBeenActivated)
        {
            spawnIn( );
        }


    }
}
