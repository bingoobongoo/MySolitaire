using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Solitaire : MonoBehaviour
{
    private float zOffsetDraw = 0;

    public Sprite[] cardFaces;
    public Sprite cardBorder;
    public Sprite cardBack;
    public SpriteRenderer deckButtonRenderer;
    public GameObject cardPrefab;
    public GameObject[] bottomPos;
    public GameObject[] topPos;
    public GameObject drawedPos;
    public GameObject deckButton;
    public List<string> deck;
    public List<string> drawed;

    public static string[] suits = new string[] {"C", "D", "H", "S"};
    public static string[] values = new string[] {"A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K"};
    public List<string>[] bottoms;
    public List<string>[] tops;

    private List<string> bottom0 = new List<string>();
    private List<string> bottom1 = new List<string>();
    private List<string> bottom2 = new List<string>();
    private List<string> bottom3 = new List<string>();
    private List<string> bottom4 = new List<string>();
    private List<string> bottom5 = new List<string>();
    private List<string> bottom6 = new List<string>();

    private List<string> top0 = new List<string>();
    private List<string> top1 = new List<string>();
    private List<string> top2 = new List<string>();
    private List<string> top3 = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        bottoms = new List<string>[] {bottom0, bottom1, bottom2, bottom3, bottom4, bottom5, bottom6};
        tops = new List<string>[] {top0, top1, top2, top3};
        deckButtonRenderer = deckButton.GetComponent<SpriteRenderer>();
        PlayCards();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayCards()
    {
        deck = GenerateDeck();
        Shuffle(deck);

        foreach (string card in deck)
            Debug.Log(card);

        Sort();
        StartCoroutine(Deal());
    }

    public static List<string> GenerateDeck()
    {
        List<string> newDeck = new List<string>();

        foreach (string s in suits)
        {
            foreach (string v in values)
            {
                newDeck.Add(s + v);
            }
        }

        return newDeck;
    }

    void Shuffle<T>(List<T> list)
    {
        System.Random random = new System.Random();
        int n = list.Count;

        while (n>1)
        {
            int k = random.Next(n);
            n--;
            T temp = list[k];
            list[k] = list[n];
            list[n] = temp;
        }
    }

    IEnumerator Deal()
    {
        for (int i=0; i<7; i++)
        {
            float yOffset = 0;
            float zOffset = 0.03f;

            foreach (string card in bottoms[i])
            {
                yield return new WaitForSeconds(0.05f);

                GameObject newCard = Instantiate(cardPrefab, new Vector3(bottomPos[i].transform.position.x, bottomPos[i].transform.position.y - yOffset, bottomPos[i].transform.position.z - zOffset), Quaternion.identity, bottomPos[i].transform);
                newCard.name = card;
                newCard.GetComponent<Selectable>().row = i;
                if (card == bottoms[i][bottoms[i].Count - 1])
                {
                    newCard.GetComponent<Selectable>().faceUp = true;
                }
                else
                {
                    newCard.GetComponent<Selectable>().faceUp = false;
                }

                newCard.GetComponent<Selectable>().setAttributes(card);
                newCard.GetComponent<Selectable>().inDeck = false;
                yOffset += 0.3f;
                zOffset += 0.03f;
            }
        }
    }

    void Sort()
    {
        for (int i=0; i<7; i++)
        {
            for (int j=i; j<7; j++)
            {
                bottoms[j].Add(deck.Last<string>()); 
                deck.RemoveAt(deck.Count - 1);
            }

        }
    }

    public void Draw()
    {
        if (deck.Count != 0)
        {
            if (deck.Count == 1)
            {
                deckButtonRenderer.sprite = cardBorder;
            }

            drawed.Add(deck.Last<string>());
            deck.RemoveAt(deck.Count - 1);
            zOffsetDraw += 0.03f;

            GameObject drawedCard = Instantiate(cardPrefab, new Vector3(drawedPos.transform.position.x, drawedPos.transform.position.y, drawedPos.transform.position.z - zOffsetDraw), Quaternion.identity);
            drawedCard.name = drawed[drawed.Count - 1];
            drawedCard.GetComponent<Selectable>().faceUp = true;
            drawedCard.GetComponent<Selectable>().inDeck = true;
            drawedCard.GetComponent<Selectable>().setAttributes(drawedCard.name);
            drawedCard.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            drawedCard.transform.SetParent(GameObject.Find("Drawed").transform);
        }

        else
        {
            deckButtonRenderer.sprite = cardBack;
            zOffsetDraw = 0;

            for (int i=drawed.Count-1; i>=0; i--)
            {
                deck.Add(drawed.Last<string>());
                Destroy(GameObject.Find(drawed[i]));
                drawed.RemoveAt(i);
            }
        }
    }

    public void Stack(GameObject firstCard, GameObject destinationCard)
    {
        // move if first and destination are both in bottom piles
        if (firstCard.GetComponent<Selectable>().inBottom == true && destinationCard.GetComponent<Selectable>().inBottom == true && firstCard.GetComponent<Selectable>().isRed != destinationCard.GetComponent<Selectable>().isRed && firstCard.GetComponent<Selectable>().value == destinationCard.GetComponent<Selectable>().value - 1 && destinationCard.GetComponent<Selectable>().checkIfFirst(destinationCard) == true)
        {
            // oblicza ilosc kart do przeniesienia w ruchu
            int n = bottoms[firstCard.GetComponent<Selectable>().row].Count - bottoms[firstCard.GetComponent<Selectable>().row].IndexOf(firstCard.name);

            Debug.Log("Ile kart w first przed ruchem: " + bottoms[firstCard.GetComponent<Selectable>().row].Count);
            Debug.Log("Index wybranej karty: " + bottoms[firstCard.GetComponent<Selectable>().row].IndexOf(firstCard.name));
            Debug.Log("Ile kart do przeniesienia: " + n);

            // usuwa n elementów talbicy z bottom od firstCard i dodaje te elementy tablicy do bottom od destinationCard
            IEnumerable<string> lista = bottoms[firstCard.GetComponent<Selectable>().row].GetRange(bottoms[firstCard.GetComponent<Selectable>().row].IndexOf(firstCard.name), n);
            bottoms[firstCard.GetComponent<Selectable>().row].RemoveRange(bottoms[firstCard.GetComponent<Selectable>().row].IndexOf(firstCard.name), n); 
            bottoms[destinationCard.GetComponent<Selectable>().row].AddRange(lista);

            firstCard.GetComponent<Selectable>().row = destinationCard.GetComponent<Selectable>().row;
            firstCard.transform.position = new Vector3(destinationCard.transform.position.x, destinationCard.transform.position.y - 0.3f, destinationCard.transform.position.z - 0.03f);
            firstCard.transform.parent = destinationCard.transform;

            Debug.Log("Ile kart zostało po przeniesieniu w first: " + bottoms[firstCard.GetComponent<Selectable>().row].Count);

            
            firstCard.GetComponent<Selectable>().checkState();

            Debug.Log("Jaka karta jest najnizej w rzędzie destination: " + bottoms[destinationCard.GetComponent<Selectable>().row][bottoms[destinationCard.GetComponent<Selectable>().row].Count - 1]);
            Debug.Log("Ile kart zostało po przeniesieniu w destination: " + bottoms[destinationCard.GetComponent<Selectable>().row].Count);
            Debug.Log(firstCard.GetComponent<Selectable>().checkIfFirst(firstCard));
 
        }

        // if first card is in drawed pile, and destination card is in bottom
        else if (firstCard.GetComponent<Selectable>().inDeck == true && destinationCard.GetComponent<Selectable>().inBottom == true && firstCard.GetComponent<Selectable>().isRed != destinationCard.GetComponent<Selectable>().isRed && firstCard.GetComponent<Selectable>().value == destinationCard.GetComponent<Selectable>().value - 1 && destinationCard.GetComponent<Selectable>().checkIfFirst(destinationCard) == true)
        {
            bottoms[destinationCard.GetComponent<Selectable>().row].Add(drawed[drawed.Count - 1]);
            drawed.RemoveAt(drawed.Count - 1);

            firstCard.GetComponent<Selectable>().row = destinationCard.GetComponent<Selectable>().row;
            firstCard.transform.position = new Vector3(destinationCard.transform.position.x, destinationCard.transform.position.y - 0.3f, destinationCard.transform.position.z - 0.03f);
            firstCard.transform.parent = destinationCard.transform;
            firstCard.GetComponent<Selectable>().checkState();
            Debug.Log("Jaka karta jest najnizej w rzędzie destination: " + bottoms[destinationCard.GetComponent<Selectable>().row][bottoms[destinationCard.GetComponent<Selectable>().row].Count - 1]);
            Debug.Log("Ile kart zostało po przeniesieniu w destination: " + bottoms[destinationCard.GetComponent<Selectable>().row].Count);
        }
        
        // if first card is in bottom and destination is in top
        else if (firstCard.GetComponent<Selectable>().inBottom == true && destinationCard.GetComponent<Selectable>().inTop == true && firstCard.GetComponent<Selectable>().checkIfFirst(firstCard) == true && firstCard.GetComponent<Selectable>().value == destinationCard.GetComponent<Selectable>().value + 1)
        {
            // if first card matches stack rules for hearts
            if (firstCard.name.Contains("H") && destinationCard.name.Contains("H"))
            {
                top0.Add(firstCard.name);
                bottoms[firstCard.GetComponent<Selectable>().row].RemoveAt(bottoms[firstCard.GetComponent<Selectable>().row].Count - 1);
                firstCard.transform.position = new Vector3(destinationCard.transform.position.x, destinationCard.transform.position.y, destinationCard.transform.position.z - 0.03f);
                firstCard.transform.parent = destinationCard.transform;
                firstCard.GetComponent<Selectable>().checkState();
            }

            // if first card matches stack rules for diamonds
            else if (firstCard.name.Contains("D") && destinationCard.name.Contains("D") )
            {
                top1.Add(firstCard.name);
                bottoms[firstCard.GetComponent<Selectable>().row].RemoveAt(bottoms[firstCard.GetComponent<Selectable>().row].Count - 1);
                firstCard.transform.position = new Vector3(destinationCard.transform.position.x, destinationCard.transform.position.y, destinationCard.transform.position.z - 0.03f);
                firstCard.transform.parent = destinationCard.transform;
                firstCard.GetComponent<Selectable>().checkState();
            }

            // if first card matches stack rules for clubs
            else if (firstCard.name.Contains("C") && destinationCard.name.Contains("C") )
            {
                top2.Add(firstCard.name);
                bottoms[firstCard.GetComponent<Selectable>().row].RemoveAt(bottoms[firstCard.GetComponent<Selectable>().row].Count - 1);
                firstCard.transform.position = new Vector3(destinationCard.transform.position.x, destinationCard.transform.position.y, destinationCard.transform.position.z - 0.03f);
                firstCard.transform.parent = destinationCard.transform;
                firstCard.GetComponent<Selectable>().checkState();
            }

            // if first card matches stack rules for spades
            else if (firstCard.name.Contains("S") && destinationCard.name.Contains("S") )
            {
                top3.Add(firstCard.name);
                bottoms[firstCard.GetComponent<Selectable>().row].RemoveAt(bottoms[firstCard.GetComponent<Selectable>().row].Count - 1);
                firstCard.transform.position = new Vector3(destinationCard.transform.position.x, destinationCard.transform.position.y, destinationCard.transform.position.z - 0.03f);
                firstCard.transform.parent = destinationCard.transform;
                firstCard.GetComponent<Selectable>().checkState();
            }

            
        }

        // if first card is in drawed pile and destination card is in top
        else if (firstCard.GetComponent<Selectable>().inDeck == true && destinationCard.GetComponent<Selectable>().inTop == true && firstCard.GetComponent<Selectable>().value == destinationCard.GetComponent<Selectable>().value + 1)
        {
            // if first card matches stack rules for hearts
            if (firstCard.name.Contains("H") && destinationCard.name.Contains("H"))
            {
                top0.Add(firstCard.name);
                drawed.RemoveAt(drawed.Count - 1);
                firstCard.transform.position = new Vector3(destinationCard.transform.position.x, destinationCard.transform.position.y, destinationCard.transform.position.z - 0.03f);
                firstCard.transform.parent = destinationCard.transform;
                firstCard.GetComponent<Selectable>().checkState();
            }

            else if (firstCard.name.Contains("D") && destinationCard.name.Contains("D"))
            {
                top1.Add(firstCard.name);
                drawed.RemoveAt(drawed.Count - 1);
                firstCard.transform.position = new Vector3(destinationCard.transform.position.x, destinationCard.transform.position.y, destinationCard.transform.position.z - 0.03f);
                firstCard.transform.parent = destinationCard.transform;
                firstCard.GetComponent<Selectable>().checkState();
            }

            else if (firstCard.name.Contains("C") && destinationCard.name.Contains("C"))
            {
                top2.Add(firstCard.name);
                drawed.RemoveAt(drawed.Count - 1);
                firstCard.transform.position = new Vector3(destinationCard.transform.position.x, destinationCard.transform.position.y, destinationCard.transform.position.z - 0.03f);
                firstCard.transform.parent = destinationCard.transform;
                firstCard.GetComponent<Selectable>().checkState();
            }

            else if (firstCard.name.Contains("S") && destinationCard.name.Contains("S"))
            {
                top3.Add(firstCard.name);
                drawed.RemoveAt(drawed.Count - 1);
                firstCard.transform.position = new Vector3(destinationCard.transform.position.x, destinationCard.transform.position.y, destinationCard.transform.position.z - 0.03f);
                firstCard.transform.parent = destinationCard.transform;
                firstCard.GetComponent<Selectable>().checkState();
            }

        }
    }

    public void StackEmpty(GameObject firstCard, GameObject destinationCard)
    {
        // if firstCard is a king from bottom 
        if (firstCard.GetComponent<Selectable>().value == 13 && firstCard.GetComponent<Selectable>().inBottom == true)
        {
            // oblicza ilosc kart do przeniesienia w ruchu
            int n = bottoms[firstCard.GetComponent<Selectable>().row].Count - bottoms[firstCard.GetComponent<Selectable>().row].IndexOf(firstCard.name);

            Debug.Log("Ile kart w first przed ruchem: " + bottoms[firstCard.GetComponent<Selectable>().row].Count);
            Debug.Log("Index wybranej karty: " + bottoms[firstCard.GetComponent<Selectable>().row].IndexOf(firstCard.name));
            Debug.Log("Ile kart do przeniesienia: " + n);

            // usuwa n elementów talbicy z bottom od firstCard i dodaje te elementy tablicy do bottom od destinationCard
            IEnumerable<string> lista = bottoms[firstCard.GetComponent<Selectable>().row].GetRange(bottoms[firstCard.GetComponent<Selectable>().row].IndexOf(firstCard.name), n);
            bottoms[firstCard.GetComponent<Selectable>().row].RemoveRange(bottoms[firstCard.GetComponent<Selectable>().row].IndexOf(firstCard.name), n); 
            bottoms[destinationCard.GetComponent<Selectable>().row].AddRange(lista);

            firstCard.transform.position = new Vector3(destinationCard.transform.position.x, destinationCard.transform.position.y, destinationCard.transform.position.z - 0.03f);
            firstCard.transform.parent = destinationCard.transform;

            Debug.Log("Ile kart zostało po przeniesieniu w first: " + bottoms[firstCard.GetComponent<Selectable>().row].Count);

            firstCard.GetComponent<Selectable>().row = destinationCard.GetComponent<Selectable>().row;
            firstCard.GetComponent<Selectable>().checkState();

            Debug.Log("Jaka karta jest najnizej w rzędzie destination: " + bottoms[destinationCard.GetComponent<Selectable>().row][bottoms[destinationCard.GetComponent<Selectable>().row].Count - 1]);
            Debug.Log("Ile kart zostało po przeniesieniu w destination: " + bottoms[destinationCard.GetComponent<Selectable>().row].Count);
            Debug.Log(firstCard.GetComponent<Selectable>().checkIfFirst(firstCard));

        }

        // if first card is king from drawed pile
        else if (firstCard.GetComponent<Selectable>().value == 13 && firstCard.GetComponent<Selectable>().inDeck == true)
        {
            bottoms[destinationCard.GetComponent<Selectable>().row].Add(drawed[drawed.Count - 1]);
            drawed.RemoveAt(drawed.Count - 1);

            firstCard.transform.position = new Vector3(destinationCard.transform.position.x, destinationCard.transform.position.y, destinationCard.transform.position.z - 0.03f);
            firstCard.transform.parent = destinationCard.transform;
            firstCard.GetComponent<Selectable>().row = destinationCard.GetComponent<Selectable>().row;
            firstCard.GetComponent<Selectable>().checkState();
            Debug.Log("Jaka karta jest najnizej w rzędzie destination: " + bottoms[destinationCard.GetComponent<Selectable>().row][bottoms[destinationCard.GetComponent<Selectable>().row].Count - 1]);
            Debug.Log("Ile kart zostało po przeniesieniu w destination: " + bottoms[destinationCard.GetComponent<Selectable>().row].Count);
            
        }

        // if first card is ace from bottoms
        else if (firstCard.GetComponent<Selectable>().value == 1 && firstCard.GetComponent<Selectable>().inBottom == true)
        {
            // hearts
            if (firstCard.name.Contains("H") && destinationCard.name == "Top0")
            {
                top0.Add(firstCard.name);
                bottoms[firstCard.GetComponent<Selectable>().row].RemoveAt(bottoms[firstCard.GetComponent<Selectable>().row].IndexOf(firstCard.name));
                firstCard.transform.position = new Vector3(destinationCard.transform.position.x, destinationCard.transform.position.y, destinationCard.transform.position.z - 0.03f);
                firstCard.transform.parent = destinationCard.transform;
                firstCard.GetComponent<Selectable>().row = destinationCard.GetComponent<Selectable>().row;
                firstCard.GetComponent<Selectable>().checkState();
            }
                
            // diamonds
            else if (firstCard.name.Contains("D")  && destinationCard.name == "Top1")
            {
                top1.Add(firstCard.name);
                bottoms[firstCard.GetComponent<Selectable>().row].RemoveAt(bottoms[firstCard.GetComponent<Selectable>().row].IndexOf(firstCard.name));
                firstCard.transform.position = new Vector3(destinationCard.transform.position.x, destinationCard.transform.position.y, destinationCard.transform.position.z - 0.03f);
                firstCard.transform.parent = destinationCard.transform;
                firstCard.GetComponent<Selectable>().row = destinationCard.GetComponent<Selectable>().row;
                firstCard.GetComponent<Selectable>().checkState();
            }
                
            // clubs
            else if (firstCard.name.Contains("C") && destinationCard.name == "Top2")
            {
                top2.Add(firstCard.name);
                bottoms[firstCard.GetComponent<Selectable>().row].RemoveAt(bottoms[firstCard.GetComponent<Selectable>().row].IndexOf(firstCard.name));
                firstCard.transform.position = new Vector3(destinationCard.transform.position.x, destinationCard.transform.position.y, destinationCard.transform.position.z - 0.03f);
                firstCard.transform.parent = destinationCard.transform;
                firstCard.GetComponent<Selectable>().row = destinationCard.GetComponent<Selectable>().row;
                firstCard.GetComponent<Selectable>().checkState();
            }
                
            // spades
            else if (firstCard.name.Contains("S") && destinationCard.name == "Top3")
            {
                top3.Add(firstCard.name);
                bottoms[firstCard.GetComponent<Selectable>().row].RemoveAt(bottoms[firstCard.GetComponent<Selectable>().row].IndexOf(firstCard.name));
                firstCard.transform.position = new Vector3(destinationCard.transform.position.x, destinationCard.transform.position.y, destinationCard.transform.position.z - 0.03f);
                firstCard.transform.parent = destinationCard.transform;
                firstCard.GetComponent<Selectable>().row = destinationCard.GetComponent<Selectable>().row;
                firstCard.GetComponent<Selectable>().checkState();
            }

            
        }

        // if first card is ace from drawed pile
        else if (firstCard.GetComponent<Selectable>().value == 1 && firstCard.GetComponent<Selectable>().inDeck == true)
        {
            // hearts
            if (firstCard.name.Contains("H") && destinationCard.name == "Top0")
            {
                top0.Add(firstCard.name);
                drawed.RemoveAt(drawed.Count - 1);
                firstCard.transform.position = new Vector3(destinationCard.transform.position.x, destinationCard.transform.position.y, destinationCard.transform.position.z - 0.03f);
                firstCard.transform.parent = destinationCard.transform;
                firstCard.GetComponent<Selectable>().row = destinationCard.GetComponent<Selectable>().row;
                firstCard.GetComponent<Selectable>().checkState();
            }
                
            // diamonds
            else if (firstCard.name.Contains("D")  && destinationCard.name == "Top1")
            {
                top1.Add(firstCard.name);
                drawed.RemoveAt(drawed.Count - 1);
                firstCard.transform.position = new Vector3(destinationCard.transform.position.x, destinationCard.transform.position.y, destinationCard.transform.position.z - 0.03f);
                firstCard.transform.parent = destinationCard.transform;
                firstCard.GetComponent<Selectable>().row = destinationCard.GetComponent<Selectable>().row;
                firstCard.GetComponent<Selectable>().checkState();
            }
                
            // clubs
            else if (firstCard.name.Contains("C") && destinationCard.name == "Top2")
            {
                top2.Add(firstCard.name);
                drawed.RemoveAt(drawed.Count - 1);
                firstCard.transform.position = new Vector3(destinationCard.transform.position.x, destinationCard.transform.position.y, destinationCard.transform.position.z - 0.03f);
                firstCard.transform.parent = destinationCard.transform;
                firstCard.GetComponent<Selectable>().row = destinationCard.GetComponent<Selectable>().row;
                firstCard.GetComponent<Selectable>().checkState();
            }
                
            // spades
            else if (firstCard.name.Contains("S") && destinationCard.name == "Top3")
            {
                top3.Add(firstCard.name);
                drawed.RemoveAt(drawed.Count - 1);
                firstCard.transform.position = new Vector3(destinationCard.transform.position.x, destinationCard.transform.position.y, destinationCard.transform.position.z - 0.03f);
                firstCard.transform.parent = destinationCard.transform;
                firstCard.GetComponent<Selectable>().row = destinationCard.GetComponent<Selectable>().row;
                firstCard.GetComponent<Selectable>().checkState();
            }

            

        }
    }
}
