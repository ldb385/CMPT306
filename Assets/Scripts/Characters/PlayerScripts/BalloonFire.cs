using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonFire : MonoBehaviour
{
    private int HeightState = 0;
    public int radius = 2;
    public ParticleSystem emitter;
    // Update is called once per frame
    void Update()
    {
        // simulates the height change 
        switch (HeightState)
        {
            default:
            case 0:
                // enlarge
                transform.localScale += Vector3.one * 7f * Time.deltaTime;
                if (transform.localScale.x >= 2.5f) HeightState = 1;
                break;
            case 1:
                // shrink to norm
                transform.localScale -= Vector3.one * 7f * Time.deltaTime;
                if (transform.localScale.x <= 1f) HeightState = 2;
                break;
            case 2:
                // fall and break
                Destroy(gameObject);
                ParticleSystem part = Instantiate(emitter, transform.position, Quaternion.identity);
                part.transform.position = transform.position;
                part.Play();

                Vector3 explosionPos = transform.position;
                Collider2D[] colliders = Physics2D.OverlapCircleAll(explosionPos, radius);
                foreach (Collider2D hit in colliders)
                {
                    if (hit.gameObject.tag == "Enemies" || hit.gameObject.tag == "Boss")
                    {
                        int BalloonDamage = GameObject.Find("Player").GetComponent<Player>().BalloonDMG;
                        hit.SendMessage("ApplyDamage", BalloonDamage);
                    }
                }
                break;

        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "wall")
        {
            Physics2D.IgnoreCollision( collision.collider, this.GetComponent<Collider2D>());
        }
    }

    // to see the radius of the balloon (doesnt work well but not important)
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Use the same vars you use to draw your Overlap SPhere to draw your Wire Sphere.
        Gizmos.DrawWireSphere(transform.position + transform.position, radius);
    }
}
