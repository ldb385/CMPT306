using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    
    // Update is called once per frame
    void Update()
    {
        // destroy projectile after 3 seconds if it hasn't hit anything
        Destroy(gameObject, 3.0f);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {

        if(collision.gameObject.tag == "Enemies" || collision.gameObject.tag == "Boss" ){
            float FireballDMG = GameObject.Find("Player").GetComponent<Player>().FireballDMG;
            collision.gameObject.SendMessage("ApplyDamage", FireballDMG);
            Destroy(this.gameObject);
        }
        else if (collision.gameObject.tag == "Player")
        {
            Physics2D.IgnoreCollision( collision.collider, this.GetComponent<Collider2D>());
        }
        else{
            Destroy(this.gameObject);
        }

    }

}
