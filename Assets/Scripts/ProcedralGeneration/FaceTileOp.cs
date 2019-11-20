using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceTileOp : MonoBehaviour
{
    private SpriteRenderer m_SpriteRenderer;

    void Start()
    {
        m_SpriteRenderer = this.GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            // change increase opacity
            m_SpriteRenderer.color = new Color(1f, 1f, 1f, 0.7f);
        }

    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            // change increase opacity
            m_SpriteRenderer.color = new Color(1f, 1f, 1f, 1f);
        }
    }
}
