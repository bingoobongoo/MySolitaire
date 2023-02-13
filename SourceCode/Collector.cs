using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collector : MonoBehaviour
{
    string ASTEROID_TAG = "Asteroid";

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(ASTEROID_TAG))
        {
            Destroy(collision.gameObject);
        }

    }  
}
