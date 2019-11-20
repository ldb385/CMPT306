using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossData : MonoBehaviour
{
    public float BossHealth = 50;
    public int BossDamage = 5;

    private Animator anim;

    private void Start() {
        anim = GetComponent<Animator>();
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
        if(BossHealth <= 25){
            anim.SetTrigger("stagetwo");
        }else if(BossHealth <=0 ){
            anim.SetTrigger("Death");
        }

    }



}
