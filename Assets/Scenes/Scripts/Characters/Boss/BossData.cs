using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossData : MonoBehaviour
{
    public float BossHealth = 50;
    public int BossDamage = 5;



    public void ApplyDamage(int Damage)
    {
        BossHealth -= Damage;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Projectile"))
        {
            BossHealth -= GameObject.Find("Player").GetComponent<Player>().FireballDMG;
        }
    }



}
