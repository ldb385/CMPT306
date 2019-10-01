using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootOnClick : MonoBehaviour
{
	public GameObject PlayerProjectile;

    void Start()
    {

    }

    void Update()
    {
		if (Input.GetMouseButtonDown(0))
		{
			Shoot();
		}
	}

	void Shoot()
	{
		Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		clickPosition.z = 0;
		Instantiate(PlayerProjectile, clickPosition, Quaternion.identity);
	}
}