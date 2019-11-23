using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class vaseDrop : MonoBehaviour
{
	[SerializeField] private GameObject largePoint;
    [SerializeField] private GameObject mediumPoint;
    [SerializeField] private GameObject smallPoint;
	
		private bool blowUp = false;
		private int drop;
	
	// detect if hit by projectile
    void OnCollisionEnter2D(Collision2D col)
    {
		blowUp = true;
    }
    
    // Raandomly selects a canday
	public void Candy()
	{
		drop =(int) Random.Range( 0, 2 );
			if( drop == 0 )
			{
				Instantiate(smallPoint,transform.position, Quaternion.identity);
			}
			else if( drop == 1 )
			{
				Instantiate(mediumPoint,transform.position, Quaternion.identity);
			}
			else
			{
				Instantiate(largePoint,transform.position, Quaternion.identity);
			}
	}

    void Update()
    {
		if(blowUp == true)
		{
			Destroy(gameObject);
			// Spawn candy drop
			Candy();
		}
    }
}
