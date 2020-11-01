using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deadLock : MonoBehaviour
{
    hexagonGrid hexGrid;
    FindMatches find;
    GameObject currentHex;
    hexagon.otherHexes currentNeighbours;

    public bool isDeadLocked = false;
    private void Start()
    {
        hexGrid = FindObjectOfType<hexagonGrid>();
        find = FindObjectOfType<FindMatches>();
    }

    public bool isLocked() //get every hexagons neighbours and test them. After that returns if there is deadlock or not
    {
        for (int i = 0; i < hexGrid.GridWidth; i++)
        {
            for (int j = 0; j < hexGrid.GridHeight; j++)
            {
                if (hexGrid.allHexagons[i, j] != null)
                {
                    currentHex = hexGrid.allHexagons[i, j];
                    currentNeighbours = currentHex.GetComponent<hexagon>().getNeighbours();

                    if(controlNeighbours(i,j,currentNeighbours)== true)
                    {
                        isDeadLocked = false;
                        return false;
                    }
                    
                }
            }
        }
        isDeadLocked = true;
        return true;
    }

    private bool controlNeighbours(int x, int y, hexagon.otherHexes hexes) //if a neighbour exists calls a switchAndMatchForLock function for it.
    {
        if(hexes.up.x >= 0 && hexes.up.x < hexGrid.GridWidth && hexes.up.y >= 0 && hexes.up.y < hexGrid.GridHeight)
        {
            if(switchAndMatchForLock(x,y,hexes.up)==true)
            {
                return true;
            }
        }
        else if (hexes.upRight.x >= 0 && hexes.upRight.x < hexGrid.GridWidth && hexes.upRight.y >= 0 && hexes.upRight.y < hexGrid.GridHeight)
        {
            if (switchAndMatchForLock(x, y, hexes.upRight) == true)
            {
                return true;
            }
        }
       
        else if (hexes.downRight.x >= 0 && hexes.downRight.x < hexGrid.GridWidth && hexes.downRight.y >= 0 && hexes.downRight.y < hexGrid.GridHeight)
        {
            if (switchAndMatchForLock(x, y, hexes.downRight) == true)
            {
                return true;
            }
        }
        else if (hexes.down.x >= 0 && hexes.down.x < hexGrid.GridWidth && hexes.down.y >= 0 && hexes.down.y < hexGrid.GridHeight)
        {
            if (switchAndMatchForLock(x, y, hexes.down) == true)
            {
                return true;
            }
        }
        else if (hexes.downLeft.x >= 0 && hexes.downLeft.x < hexGrid.GridWidth && hexes.downLeft.y >= 0 && hexes.downLeft.y < hexGrid.GridHeight)
        {
            if (switchAndMatchForLock(x, y, hexes.downLeft) == true)
            {
                return true;
            }
        }
        else if (hexes.upLeft.x >= 0 && hexes.upLeft.x < hexGrid.GridWidth && hexes.upLeft.y >= 0 && hexes.upLeft.y < hexGrid.GridHeight)
        {
            if (switchAndMatchForLock(x, y, hexes.upLeft) == true)
            {
                return true;
            }
        }
        return false;
    }
    private bool switchAndMatchForLock(int x, int y, Vector2 hexToMove) //switch currentHex with selected neighbour and control if there is a match or not after that switch back to original position
    {
        switchForLock(x, y, hexToMove);
        if (controlMatchesForLock(x,y) == true)
        {
            switchForLock(x, y, hexToMove);    
            return true;
        }
        switchForLock(x, y, hexToMove);
        return false;
    }
    private void switchForLock(int x, int y, Vector2 hex) //function for switching currentHex and selected neighbour
    {
        GameObject tmpHex = hexGrid.allHexagons[(int)hex.x, (int)hex.y] as GameObject;
        hexGrid.allHexagons[(int)hex.x,(int)hex.y] = hexGrid.allHexagons[x, y];
        hexGrid.allHexagons[x, y] = tmpHex;
    }  
    private bool controlMatchesForLock(int x,int y) //controls if there is a match or not
    {
        GameObject controlHex;
        hexagon.otherHexes controlNeighbours;
        List<GameObject> controlNeighbourList = new List<GameObject>();
        controlHex = hexGrid.allHexagons[x, y];

        controlNeighbours = controlHex.GetComponent<hexagon>().getNeighbours();

        if (controlNeighbours.up.x >= 0 && controlNeighbours.up.x < hexGrid.GridWidth && controlNeighbours.up.y >= 0 && controlNeighbours.up.y < hexGrid.GridHeight)
        {
            controlNeighbourList.Add(hexGrid.allHexagons[(int)controlNeighbours.up.x, (int)controlNeighbours.up.y]);
        }
        else controlNeighbourList.Add(null);

        if (controlNeighbours.upRight.x >= 0 && controlNeighbours.upRight.x < hexGrid.GridWidth && controlNeighbours.upRight.y >= 0 && controlNeighbours.upRight.y < hexGrid.GridHeight)
        {
            controlNeighbourList.Add(hexGrid.allHexagons[(int)controlNeighbours.upRight.x, (int)controlNeighbours.upRight.y]);
        }
        else controlNeighbourList.Add(null);

        if (controlNeighbours.downRight.x >= 0 && controlNeighbours.downRight.x < hexGrid.GridWidth && controlNeighbours.downRight.y >= 0 && controlNeighbours.downRight.y < hexGrid.GridHeight)
        {
            controlNeighbourList.Add(hexGrid.allHexagons[(int)controlNeighbours.downRight.x, (int)controlNeighbours.downRight.y]);
        }
        else controlNeighbourList.Add(null);

        if (controlNeighbours.down.x >= 0 && controlNeighbours.down.x < hexGrid.GridWidth && controlNeighbours.down.y >= 0 && controlNeighbours.down.y < hexGrid.GridHeight)
        {
            controlNeighbourList.Add(hexGrid.allHexagons[(int)controlNeighbours.down.x, (int)controlNeighbours.down.y]);
        }
        else controlNeighbourList.Add(null);

        if (controlNeighbours.downLeft.x >= 0 && controlNeighbours.downLeft.x < hexGrid.GridWidth && controlNeighbours.downLeft.y >= 0 && controlNeighbours.downLeft.y < hexGrid.GridHeight)
        {
            controlNeighbourList.Add(hexGrid.allHexagons[(int)controlNeighbours.downLeft.x, (int)controlNeighbours.downLeft.y]);
        }
        else controlNeighbourList.Add(null);

        if (controlNeighbours.upLeft.x >= 0 && controlNeighbours.upLeft.x < hexGrid.GridWidth && controlNeighbours.upLeft.y >= 0 && controlNeighbours.upLeft.y < hexGrid.GridHeight)
        {
            controlNeighbourList.Add(hexGrid.allHexagons[(int)controlNeighbours.upLeft.x, (int)controlNeighbours.upLeft.y]);
        }
        else controlNeighbourList.Add(null);


        for (int k = 0; k < controlNeighbourList.Count-1; k++)
        {          
            if (controlNeighbourList[k] != null && controlNeighbourList[k + 1] != null)
            {
                if (controlNeighbourList[k].tag == currentHex.tag && controlNeighbourList[k + 1].tag == currentHex.tag)
                {
                    return true;
                }
            }
        }
        if (controlNeighbourList[0] != null && controlNeighbourList[controlNeighbourList.Count - 1] != null)
        {
            if (controlNeighbourList[0].tag == currentHex.tag && controlNeighbourList[controlNeighbourList.Count - 1].tag == currentHex.tag)
            {
                return true;
            }
        }
        return false;
    }  
   
}
