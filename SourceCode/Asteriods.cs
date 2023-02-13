using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteriods : MonoBehaviour
{
    [HideInInspector]
    public float speed, rotationForce;
    private Rigidbody2D myBody; 
    Quaternion currentRotation;
    Vector3 currentEulerAngles = new Vector3(0f, 0f, 0f);

    void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        speed = -2f;
        rotationForce = 5f;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        myBody.velocity = new Vector2(speed, myBody.velocity.y);
        RotateAsteroid();
        
    }

    void RotateAsteroid()
    {
        currentEulerAngles += new Vector3(0f, 0f, rotationForce) * Time.deltaTime;
        currentRotation.eulerAngles = currentEulerAngles;
        transform.rotation = currentRotation;
    }
}
