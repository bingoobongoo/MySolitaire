using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [HideInInspector]
    public float laserSpeed = 20f;
    Rigidbody2D myBody;

    [SerializeField]
    GameObject explosionReference;
    GameObject explosion;

    void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        StartCoroutine(autoDestruction());
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        myBody.velocity = new Vector2(laserSpeed, myBody.velocity.y);
    }

    IEnumerator startExplosion()
    {
        explosion = Instantiate(explosionReference);
        explosion.transform.position = transform.position;
        Destroy(gameObject);
        yield return new WaitForSeconds(1);
        Destroy(explosion);
    }

    public IEnumerator autoDestruction()
    {
        yield return new WaitForSeconds(0.7f);
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            Destroy(collision.gameObject);
            StartCoroutine(startExplosion());
        }
    }
    
}
