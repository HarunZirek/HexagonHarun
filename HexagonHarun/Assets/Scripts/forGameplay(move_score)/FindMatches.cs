using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class FindMatches : MonoBehaviour
{
    private hexagonGrid hexGrid;

    // a list for matches in the grid
    public List<GameObject> matches = new List<GameObject>();
    
    void Start()
    {
        hexGrid = FindObjectOfType<hexagonGrid>();
    }
    public void findAllMatches()
    {
        findMatches();
    }

    private void addToList(GameObject hex) //adds the matching hexagon to the list of matches and replaces its isMatched value to true
    {
        if (!matches.Contains(hex))  //if list of matches contains this hexagon function wont add it 
        {
            matches.Add(hex);
        }
        hex.GetComponent<hexagon>().isMatched = true;
    }
    private void fillTheList(GameObject hex1, GameObject hex2, GameObject hex3) //calls the addToList for every matched hexagon
    {
        addToList(hex1);
        addToList(hex2);
        addToList(hex3);
    }


    /*
       The function for finding matches in the grid. Starting from [0,0] adds every hexagons up, upright and downright neighbour to neighbourList if it is available.
       The reason for only adding up, upright and downright is we start controlling from bottom left so we dont need the check bottoms and left sides.
       Every hexagon has its own tag that shows its color. I added these tags from Unity editor and i controlled matches from  hexagons tags.
       After adding neighbours to list, checks their tags and if there is a match, adds it to list of matches
       after that clears the neighbour list and current hexagon for controlling another hexagon
    */
    private void findMatches()
    {
        List<GameObject> neighbourList = new List<GameObject>();
        GameObject currentHex;
        hexagon.otherHexes currentNeighbours;


        for (int i = 0; i < hexGrid.GridWidth; i++)
        {
            for (int j = 0; j < hexGrid.GridHeight; j++)
            {
                currentHex = hexGrid.allHexagons[i,j];
                
                currentNeighbours = currentHex.GetComponent<hexagon>().getNeighbours();

                if (currentNeighbours.up.x >= 0 && currentNeighbours.up.x < hexGrid.GridWidth && currentNeighbours.up.y >= 0 && currentNeighbours.up.y < hexGrid.GridHeight)
                {
                    neighbourList.Add(hexGrid.allHexagons[(int)currentNeighbours.up.x,(int)currentNeighbours.up.y]);
                }
                else neighbourList.Add(null);

                if (currentNeighbours.upRight.x >= 0 && currentNeighbours.upRight.x < hexGrid.GridWidth && currentNeighbours.upRight.y >= 0 && currentNeighbours.upRight.y < hexGrid.GridHeight)
                {
                    neighbourList.Add(hexGrid.allHexagons[(int)currentNeighbours.upRight.x, (int)currentNeighbours.upRight.y]);
                }
                else neighbourList.Add(null);

                if (currentNeighbours.downRight.x >= 0 && currentNeighbours.downRight.x < hexGrid.GridWidth && currentNeighbours.downRight.y >= 0 && currentNeighbours.downRight.y < hexGrid.GridHeight)
                {
                    neighbourList.Add(hexGrid.allHexagons[(int)currentNeighbours.downRight.x, (int)currentNeighbours.downRight.y]);
                }
                else neighbourList.Add(null);



                for (int k = 0; k < neighbourList.Count - 1; k++)
                {
                    if (neighbourList[k] != null && neighbourList[k + 1] != null)
                    {
                        if (neighbourList[k].tag == currentHex.tag && neighbourList[k + 1].tag == currentHex.tag)
                        {
                            fillTheList(neighbourList[k], neighbourList[k + 1], currentHex);
                        }
                    }
                }
                neighbourList.Clear();
                currentHex = null;
            }
        }
    }
}
