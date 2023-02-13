using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInput : MonoBehaviour
{
    private Solitaire solitaire;
    private bool frameCooldown = false;

    // Start is called before the first frame update
    void Start()
    {
        solitaire = FindObjectOfType<Solitaire>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        GetMouseClick();
    }

    void GetMouseClick()
    {
        if (frameCooldown)
            frameCooldown = false;

        else if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -10));
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null)
            {
                GameObject selected = hit.collider.gameObject;

                if (hit)
                {
                    if (selected.CompareTag("Deck")) // hit deck
                    {
                        Deck();
                    }

                    else if (selected.CompareTag("Card") && selected.GetComponent<Selectable>().faceUp == false && selected.GetComponent<Selectable>().checkIfFirst(selected) == true)
                    {
                        RevealCard(selected);
                    }

                    else if (selected.CompareTag("Card") && selected.GetComponent<Selectable>().faceUp == true) // hit card
                    {
                        Card(selected);
                    }

                    else if (selected.CompareTag("Top")) // hit top
                    {
                        Top();
                    }

                    else if (selected.CompareTag("Bottom")) // hit bottom
                    {
                        Bottom();
                    }

                    else
                    {

                    }
                }
            }
        }
    }

    IEnumerator GetSecondMouseClick(GameObject firstCard)
    {
        do
        {
            yield return null;

        } while (!Input.GetMouseButtonDown(0));

        frameCooldown = true;

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -10));
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (hit.collider != null)
        {
            GameObject selected = hit.collider.gameObject;

            if (hit)
            {
                if (selected.CompareTag("Card"))
                {
                    solitaire.Stack(firstCard, selected);
                }

                else if (selected.CompareTag("Bottom") || selected.CompareTag("Top"))
                {
                    solitaire.StackEmpty(firstCard, selected);
                }

                firstCard.GetComponent<Selectable>().isSelected = false;
                firstCard.GetComponent<UpdateSprite>().setColor(false);

            }
        }
        
        firstCard.GetComponent<Selectable>().isSelected = false;
        firstCard.GetComponent<UpdateSprite>().setColor(false);
            
    }

    void Deck()
    {
        Debug.Log("Clicked on deck!");
        solitaire.Draw();
    }

    void Card(GameObject card)
    {
        Debug.Log("Clicked on: " + card.name);
        card.GetComponent<Selectable>().isSelected = true;
        card.GetComponent<UpdateSprite>().setColor(true); // sets tint of a card
        StartCoroutine(GetSecondMouseClick(card));
    }

    void RevealCard(GameObject card)
    {
        card.GetComponent<Selectable>().faceUp = true;
    }

    void Top()
    {
        Debug.Log("Clicked on top!");
    }

    void Bottom()
    {
        Debug.Log("Clicked on bottom!");
    }


}
