﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
	// Update is called once per frame
	void Update()
	{
		// destroy projectile after 10 seconds if it hasn't hit anything
		Destroy(gameObject, 10.0f);
	}

	public void OnCollisionEnter2D(Collision2D collision)
	{
		// add fancy effects here

		if (collision.gameObject.tag != "Enemies")
		{
			Destroy(gameObject);
		}
	}
}
