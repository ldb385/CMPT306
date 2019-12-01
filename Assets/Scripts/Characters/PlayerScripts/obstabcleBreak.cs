using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obstabcleBreak : MonoBehaviour
{
    [SerializeField] private GameObject largePoint;
    [SerializeField] private GameObject mediumPoint;
    [SerializeField] private GameObject smallPoint;
    [SerializeField] private GameObject largeParticle;
    [SerializeField] private GameObject mediumParticle;
    [SerializeField] private GameObject smallParticle;

    private bool blowUp = false;
    private int drop;
    public AudioClip obSmash;
    public float volume = 1f;
    private int hit = 0;

    // detect if hit by projectile
    void OnCollisionEnter2D( Collision2D c)
    {
        if (c.gameObject.CompareTag("Projectile")|| c.gameObject.CompareTag("EnemyProjectile"))
        {
            hit++;
            if(hit ==2)
            {
                blowUp = true;
            }
        }
    }

    // Raandomly selects a canday
    public void Candy()
    {
        drop = (int)Random.Range(0, 2);
        if (drop == 0)
        {
            Instantiate(smallParticle, transform.position, Quaternion.identity);
            Instantiate(smallPoint, transform.position, Quaternion.identity);
        }
        else if (drop == 1)
        {
            Instantiate(mediumParticle, transform.position, Quaternion.identity);
            Instantiate(mediumPoint, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(largeParticle, transform.position, Quaternion.identity);
            Instantiate(largePoint, transform.position, Quaternion.identity);
        }
    }

    void Update()
    {
        if (blowUp == true)
        {
            AudioSource.PlayClipAtPoint(obSmash, transform.position, volume);
            Destroy(gameObject);
            // Spawn candy drop
            Candy();
        }
    }
}
