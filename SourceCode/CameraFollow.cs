using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform player;
    private Vector3 tempPos;
    private string PLAYER_TAG = "Player";
    private float minX = -60f, maxX = 120f;

    [SerializeField]
    GameObject gameOverReference;
    GameObject gameOverSprite;

    bool isGameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag(PLAYER_TAG).transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (player == null)
        {
            StartCoroutine(EndGameSession());
            return;
        }

        tempPos = transform.position;
        tempPos.x = player.position.x;
        if (tempPos.x < minX)
        {
            tempPos.x = minX;
        }
        else if (tempPos.x > maxX)
        {
            tempPos.x = maxX;
        }
        transform.position = tempPos;
    }

    void PrintGameOver()
    {
        if (isGameOver == false)
            {
                gameOverSprite = Instantiate(gameOverReference);
                gameOverSprite.transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
                isGameOver = true;
            }
    }

    IEnumerator EndGameSession()
    {
        PrintGameOver();
        yield return new WaitForSeconds(5);
        MainMenu.LoadMainMenu();
    }
}
