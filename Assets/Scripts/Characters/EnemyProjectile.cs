using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
	public int ProjectileDamage = 3;
	private void Start() {
		
	}

	// Update is called once per frame
	void Update()
	{
		// destroy projectile after 10 seconds if it hasn't hit anything
		Destroy(gameObject, 3.0f);
	}

	public void OnCollisionEnter2D(Collision2D collision)
	{
		Debug.Log("Player");
		// add fancy effects here
		if(collision.gameObject.CompareTag("Player")){
			
            collision.gameObject.SendMessage("DamagePlayer", ProjectileDamage);
			Destroy(gameObject);
        }
		else if (collision.gameObject.tag != "EnemyProjectile" && collision.gameObject.tag != "Enemies" )
		{
			Destroy(gameObject);
		}

	}



}
