﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossData : MonoBehaviour
{
    public float BossHealth = 50;
    public int BossDamage = 5;
    private GameObject Player;
    public GameObject[] SpeechBubbles;
	public int projectileSpeed;
    private Animator anim;
    public int numProjectile = 100;
    public float projSpeed= 20;
    public Slider Healthbar;

    private void Start() {
        anim = GetComponent<Animator>();
        Player = GameObject.Find("Player");
    }

    public void ApplyDamage(int Damage)
    {
        BossHealth -= Damage;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "EnemyProjectile"){
            Physics2D.IgnoreCollision( other.collider, this.GetComponent<Collider2D>());
        }
    }


    private void Update() {
        Healthbar.value = BossHealth;
        if(BossHealth <= 25){
            anim.SetTrigger("stagetwo");
        }
        if(BossHealth <= 0 ){
            anim.SetTrigger("Death");
        }

    }
	public void AttackOne(){


        int rand = Random.Range(0,SpeechBubbles.Length);
		float angleStep = 360f / numProjectile;
		float angle = 0f;

		for (int i = 0; i <= numProjectile - 1; i++) {
			
			float projectileDirXposition = this.transform.position.x + Mathf.Sin ((angle * Mathf.PI) / 180) * 5f ;
			float projectileDirYposition = this.transform.position.y + Mathf.Cos ((angle * Mathf.PI) / 180) * 5f;

			Vector2 projectileVector = new Vector2 (projectileDirXposition, projectileDirYposition);
			Vector2 projectileMoveDirection = (projectileVector - (Vector2)this.transform.position).normalized * projSpeed;

			var proj = Instantiate (SpeechBubbles[rand], this.transform.position, Quaternion.identity);
			proj.GetComponent<Rigidbody2D> ().velocity = 
				new Vector2 (projectileMoveDirection.x, projectileMoveDirection.y);
            proj.GetComponent<Rigidbody2D>().angularVelocity = -1000f;

			angle += angleStep;
		}

    }


}
