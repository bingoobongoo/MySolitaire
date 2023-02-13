using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject asteroidReference;

     GameObject spawnedAsteroid;

    [SerializeField]
    Transform leftPos, rightPos;

    int randomSide;
    float randomY;
    float randomSize;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spawnAsteriod());
    }

    IEnumerator spawnAsteriod()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
            randomSide = Random.Range(0, 2);
            randomSize = Random.Range(0.8f, 2.2f);
            randomY = Random.Range(-4f, 4f);

            spawnedAsteroid = Instantiate(asteroidReference);

            if (randomSide == 0) // left side
            {   
                leftPos.position = new Vector3(leftPos.position.x, randomY, leftPos.position.z);
                spawnedAsteroid.transform.localScale = new Vector3(randomSize, randomSize, 1f);
                spawnedAsteroid.transform.position = leftPos.position;
                spawnedAsteroid.GetComponent<Asteriods>().speed = Random.Range(5f, 15f);
                spawnedAsteroid.GetComponent<Asteriods>().rotationForce = Random.Range(45f, 90f);
            }
            else // right side
            {
                rightPos.position = new Vector3(rightPos.position.x, randomY, rightPos.position.z);
                spawnedAsteroid.transform.localScale = new Vector3(-randomSize, -randomSize, 1f);
                spawnedAsteroid.transform.position = rightPos.position;
                spawnedAsteroid.GetComponent<Asteriods>().speed = Random.Range(-15f, -5f);
                spawnedAsteroid.GetComponent<Asteriods>().rotationForce = Random.Range(-90f, -45f);
            }
        }
    }
}
