using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectable : MonoBehaviour
{
    public bool faceUp = false;
    public bool isSelected = false;
    public bool inTop = false;
    public bool inBottom = false;
    public bool inDeck = false;
    public bool isRed;
    public int value;
    public int row;

    Solitaire solitaire;
    // Start is called before the first frame update
    void Start()
    {
        solitaire = FindObjectOfType<Solitaire>(); 
        checkState();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.parent && this.transform.parent.GetComponent<Selectable>())
        {
            this.GetComponent<Selectable>().row = this.transform.parent.GetComponent<Selectable>().row;
        }

        this.checkState();
        
    }

    public void setAttributes(string name)
    {
        if (name.Contains('H') || name.Contains('D'))
        {
            isRed = true;
        }

        else 
        {
            isRed = false;
        }

        if (name.EndsWith("A"))
            value = 1;
        else if (name.EndsWith("2"))
            value = 2;
        else if (name.EndsWith("3"))
            value = 3;
        else if (name.EndsWith("4"))
            value = 4;
        else if (name.EndsWith("5"))
            value = 5;
        else if (name.EndsWith("6"))
            value = 6;
        else if (name.EndsWith("7"))
            value = 7;
        else if (name.EndsWith("8"))
            value = 8;
        else if (name.EndsWith("9"))
            value = 9;
        else if (name.EndsWith("10"))
            value = 10;
        else if (name.EndsWith("J"))
            value = 11;
        else if (name.EndsWith("Q"))
            value = 12;
        else if (name.EndsWith("K"))
            value = 13;
    }

    public void checkState()
    {
        if (this.transform.parent && this.transform.parent.name == "Drawed") // check if card is in deck pile
            inDeck = true;
        else
            inDeck = false;

        if (this.row == -1) // checks if card is in one of the Top piles
            inTop = true;
        else 
            inTop = false;

        if (!inTop && !inDeck)
            inBottom = true;
        else 
            inBottom = false;
    }

    public bool checkIfFirst(GameObject card)
    {
        int lastIndex = solitaire.bottoms[this.row].Count - 1;
        if (card.name == solitaire.bottoms[this.row][lastIndex]) // checks if card is first (isn't blocked)
            return true;
        else
            return false;
    }
}
