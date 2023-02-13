using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpaceship : MonoBehaviour
{
    private float moveForce = 5f;

    private float movementX;
    private float movementY;

    float minX = -65f, maxX = 125f, minY = -4.6f, maxY = 4.6f;

    [SerializeField]
    GameObject explosionReference;
    GameObject explosion;

    [SerializeField]
    GameObject laserReference;
    GameObject laser;

    private Vector3 currentEulerAngles = new Vector3(0f, 0f, 0f);
    private Quaternion currentRotation;

    private SpriteRenderer sr;
    private Rigidbody2D myBody;
    private Animator anim;
    private string MOVE_ANIMATION = "Move";
    string ASTEROID_TAG = "Asteroid";

    bool isTurnedRight = true;

    private void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    { 
        PlayerMoveKeyboard(); 
        CheckPlayerPosition();
        AnimatePlayer();
        ShootLaser();
    }

    IEnumerator startExplosion()
    {
        explosion = Instantiate(explosionReference);
        explosion.transform.position = transform.position;
        Destroy(gameObject);
        yield return new WaitForSeconds(1);
        Destroy(explosion);
    }

    void CheckPlayerPosition()
    {
        if (transform.position.x < minX)
        {
            transform.position = new Vector3(minX, transform.position.y, transform.position.z);
        }

        if (transform.position.x > maxX)
        {
            transform.position = new Vector3(maxX, transform.position.y, transform.position.z);
        }

        if (transform.position.y < minY)
        {
            transform.position = new Vector3(transform.position.x, minY, transform.position.z);
        }

        if (transform.position.y > maxY)
        {
            transform.position = new Vector3(transform.position.x, maxY, transform.position.z);
        }

    }

    void ShootLaser()
    {
        if (Input.GetButtonDown("Jump"))
        {
            laser = Instantiate(laserReference);

            if (isTurnedRight)
            {
                laser.transform.position = new Vector3(transform.position.x + 0.5f, transform.position.y, transform.position.z );
                laser.GetComponent<Laser>().laserSpeed = 20f;
            }
            else
            {
                laser.transform.position = new Vector3(transform.position.x - 0.5f, transform.position.y, transform.position.z );
                laser.GetComponent<Laser>().laserSpeed = -20f;

            }
        }
    }

    void PlayerMoveKeyboard()
    {
        movementX = Input.GetAxisRaw("Horizontal");
        movementY = Input.GetAxisRaw("Vertical");

        transform.position += new Vector3(movementX, 0f, 0f) * Time.deltaTime * moveForce;
        transform.position += new Vector3(0f, movementY, 0f) * Time.deltaTime * moveForce;

    }

    void AnimatePlayer() 
    {
       if (movementX > 0) // move to the right
       {
            anim.SetBool(MOVE_ANIMATION, true);
            RotatePlayer("right");
            isTurnedRight = true;
       }
       else if (movementX < 0) // move to the left
       {
            anim.SetBool(MOVE_ANIMATION, true);
            RotatePlayer("left");
            isTurnedRight = false;
       }
       
       //==========================================//

       if (movementY > 0)
       {
            anim.SetBool(MOVE_ANIMATION, true);
       }
       else if (movementY < 0)
       {
            anim.SetBool(MOVE_ANIMATION, true);
       }

       if (movementX == 0 && movementY == 0)
       {
        anim.SetBool(MOVE_ANIMATION, false);
       }
    }

    void RotatePlayer(string direction)
    {
        if (direction == "right")
        {
            currentEulerAngles = new Vector3(0f, 0f, 0f);
            currentRotation.eulerAngles = currentEulerAngles;
            transform.rotation = currentRotation; 
        }
        else if (direction == "left")
        {
            currentEulerAngles = new Vector3(0f, 0f, 180f);
            currentRotation.eulerAngles = currentEulerAngles;
            transform.rotation = currentRotation;
        }
    } 

    void OnCollisionEnter2D(Collision2D collison)
    {
        if (collison.gameObject.CompareTag(ASTEROID_TAG))
        {
            StartCoroutine(startExplosion());
        }

    }  
}
