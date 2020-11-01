using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hexagonGrid : MonoBehaviour
{
    //these numbers are assinged after trying different ones in unity editor for visual
    private float sizeX = 0.8f;
    private float sizeY = 0.9f;
    private float gap = 0.45f;
    private float offSetForNewHex = 2f;

    //the bomb appears in every 1000 score. we make the bomb with dividing score by thousand so this value starts from one and increases after every bomb made.
    private int newBomb = 1;

    public int GridWidth;
    public int GridHeight;
  
    public GameObject[] HexagonColors; //color class that holds hex prefabs with different colors.
    public GameObject[,] allHexagons; //grid class that hold all hexes in the field

    private List<GameObject> selectedGroup=new List<GameObject>();

    private FindMatches find;
    private ScoreSystem scoreManager;
    public GameObject gameOverUI;
    

    private GameObject selectedHexagon;
    private int XHex;
    private int YHex;

    private int scoreForPiece = 5;
    public int SelectCount = 0;
    public bool isGameOver = false;
    private bool canRotate = true;
    public bool canPlay = true;

    public GameObject outLineHolder;
    public Sprite outlineSprite;


    void Start()
    {      
        find = FindObjectOfType<FindMatches>();
        scoreManager = FindObjectOfType<ScoreSystem>();      
        allHexagons = new GameObject[GridWidth, GridHeight]; //assign grid with width and height that can be controlled in editor
        SetUp();       
    }
    private void Update()
    {
       
        if(isGameOver==true)  //if the game over value is true opens game over screen. By default it is false.
        {
            canPlay = false;
            gameOverUI.SetActive(true);
        }
    }
    private void SetUp()  // function that sets the scene in the start
    {
        float x = 0;
        for (int i = 0; i < GridWidth; i++)
        {
            float y = 0;
            for (int j = 0; j < GridHeight; j++)
            {
                Vector2 tempPosition;  //hex's y positon depends on the hex's place in the grid. Before assing the possition we check the grid. 

                if (i % 2 == 0)  
                {
                    tempPosition = new Vector2(x, y);
                }
                else
                {
                    tempPosition = new Vector2(x, y + gap);
                }
                y += sizeY;
                int hexToUse = Random.Range(0, HexagonColors.Length);   //using random color for hexagon

                while (CheckMatchesOnStart(i, j, HexagonColors[hexToUse])) //if selected color makes a match on start selects different one
                {
                    hexToUse = Random.Range(0, HexagonColors.Length);
                }

                //generate hexagon and assign its values for grid
                GameObject hex = Instantiate(HexagonColors[hexToUse], tempPosition, Quaternion.identity) as GameObject;
                hex.GetComponent<hexagon>().x = i;
                hex.GetComponent<hexagon>().y = j;
                hex.transform.parent = this.transform;
                hex.name = "(" + i + "," + j + ")";
                allHexagons[i, j] = hex;
            }
            x += sizeX;            
        }       
    }
    private bool CheckMatchesOnStart(int x, int y, GameObject piece)  //controls the generated hex makes a match on the start.
    {
        if (x % 2 == 0)
        {
            if (x > 0 && y > 0)
            {
                if (allHexagons[x - 1, y - 1].tag == piece.tag && allHexagons[x, y - 1].tag == piece.tag)
                {
                    return true;
                }
                else if (allHexagons[x - 1, y - 1].tag == piece.tag && allHexagons[x - 1, y].tag == piece.tag)
                {
                    return true;
                }
                if (y == GridHeight - 1)
                {
                    if (allHexagons[x - 1, y - 1].tag == piece.tag && allHexagons[x - 1, y].tag == piece.tag)
                    {
                        return true;
                    }
                }
            }
        }
        else
        {
            if (y > 0 && x > 0)
            {
                if (allHexagons[x - 1, y].tag == piece.tag && allHexagons[x, y - 1].tag == piece.tag)
                {
                    return true;
                }
                if (y < GridHeight - 1)
                {
                    if (allHexagons[x - 1, y].tag == piece.tag && allHexagons[x - 1, y + 1].tag == piece.tag)
                    {
                        return true;
                    }
                }
            }
            else if (y == 0 && x > 0)
            {
                if (allHexagons[x - 1, y].tag == piece.tag && allHexagons[x - 1, y + 1].tag == piece.tag)
                {
                    return true;
                }
            }
        }
        return false;
    }



    public void DestroyMatches()  //after limiting players movement controls all the hexes in the grid and destroys the matched ones
    {
        canPlay = false;
        canRotate = false;
        destroyOutline();

        for (int i = 0; i < GridWidth; i++)
        {
            for (int j = 0; j < GridHeight; j++)
            {
                if (allHexagons[i, j] != null)
                {
                    if (allHexagons[i, j].GetComponent<hexagon>().isMatched == true)
                    {
                        Destroy(allHexagons[i, j]);
                        FindObjectOfType<audioManager>().Play("hex");
                        allHexagons[i, j] = null;
                        scoreManager.AddScore(scoreForPiece);
                    }
                }
            }
        }
        find.matches.Clear();
        StartCoroutine(collapseThePieces());
    }
    public bool matchesOnBoard()  //checks if there is a match or not
    {
        find.findAllMatches();
        if(find.matches.Count>0)
        {
            return true;
        }
        return false;
    }    
    private IEnumerator collapseThePieces() //Makes the hexagons on the top fill the gaps that made by destroyed hexagons
    {
        
        for (int i = 0; i < GridWidth; i++)
        {
            for (int j = 0; j < GridHeight; j++)
            {
                Vector3 newPos;
                if (allHexagons[i, j] == null)
                {
                    for (int k = j + 1; k < GridHeight; k++)
                    {
                        if (allHexagons[i, k] != null)
                        {
                            allHexagons[i, k].GetComponent<hexagon>().y = j;
                            allHexagons[i, j] = allHexagons[i, k];
                            if (i % 2 == 0)
                            {                             
                                newPos = new Vector3(i * sizeX, j * sizeY, 0);
                                allHexagons[i,j].GetComponent<hexagon>().changeToNewPos(newPos);
                            }
                            else
                            {
                                newPos = new Vector3(i * sizeX, (j * sizeY) + gap, 0);
                                allHexagons[i, j].GetComponent<hexagon>().changeToNewPos(newPos);
                            }
                            allHexagons[i, k] = null;
                           
                            break;
                        }
                        
                    }
                }
            }
        }
        yield return new WaitForSeconds(0.3f);
        StartCoroutine(checkBoard());
    }  
    private IEnumerator checkBoard() //after filling the grid, checks if there is a match again
    {
        refillEmpty();
        yield return new WaitForSeconds(0.3f);
        if (matchesOnBoard() == true)  //after fill we control if there is a match or not
        {  
            yield return new WaitForSeconds(0.3f);
            DestroyMatches();
        }
        else
        {
            selectedHexagon = null;
            canPlay = true;
            canRotate = true;
        }
        yield return new WaitForSeconds(0.3f);
       
    }
    private void refillEmpty()   //creating new hexagons for filling the empty places
    {
        for (int i = 0; i < GridWidth; i++)
        {
            for (int j = 0; j < GridHeight; j++)
            {
                Vector2 tempPos; //offscreen position. new hexagon generates here after that drops the new position
                Vector2 newPos;

                if (allHexagons[i, j] == null)
                {
                    if(i%2==0)
                    {
                         newPos = new Vector2(i * sizeX, j * sizeY);
                         tempPos = new Vector2(i * sizeX, (j * sizeY)+offSetForNewHex);
                    }
                    else
                    {
                         newPos = new Vector2( i * sizeX, (j* sizeY)+gap);
                         tempPos = new Vector2(i * sizeX, (j * sizeY) + gap + offSetForNewHex);
                    }


                    int color = Random.Range(0, HexagonColors.Length);
                    GameObject newHex = Instantiate(HexagonColors[color], tempPos, Quaternion.identity);
                    newHex.GetComponent<hexagon>().changeToNewPos(newPos);

                    if (scoreManager.score / 1000 == newBomb)  //checking the score for making bombs
                    {
                        newHex.GetComponent<hexagon>().isBomb = true;
                        newBomb++;
                    }
                    allHexagons[i, j] = newHex;
                    newHex.GetComponent<hexagon>().y = j;
                    newHex.GetComponent<hexagon>().x = i;                   
                }
            }
        }
       
    }



    public void selectHex(Collider2D collider) //if the canPlay value is true selects a hexagon that player touches.
    {       
        if(canPlay==true)
        {
            if (selectedHexagon == null || !selectedHexagon.GetComponent<Collider2D>().Equals(collider))
            {
                selectedHexagon = collider.gameObject.GetComponent<hexagon>().gameObject;
                XHex = selectedHexagon.GetComponent<hexagon>().x;
                YHex = selectedHexagon.GetComponent<hexagon>().y;
                SelectCount = 0;
            }
            else if (selectedHexagon.GetComponent<Collider2D>() == collider) //if player touches same hexagon again changes the SelectCount value for selecting different group
            {
                SelectCount++;
                if (SelectCount >= 6)
                {
                    SelectCount = 0;
                }
            }
            destroyOutline();
            makeOutline();
        }
    }
    private void destroyOutline() //destroys the outline that made by selecting a hex
    {
        if (outLineHolder.transform.childCount > 0)
        {
            foreach (Transform child in outLineHolder.transform)
                Destroy(child.gameObject);
        }
    } 
    private void makeOutline() //creates a outline for selected hex group
    {        
        findHexGroup();
      
        foreach (GameObject outlineHexagon in selectedGroup)
        {
            GameObject hex = outlineHexagon;
            GameObject outline = new GameObject("outLine");

            outline.transform.parent = outLineHolder.transform;

            outline.AddComponent<SpriteRenderer>();   //generated outline will added in unity editor
            outline.GetComponent<SpriteRenderer>().sprite = outlineSprite;
            outline.transform.position = new Vector3(hex.transform.position.x, hex.transform.position.y, 1); //generating outline behind the hex. In this way outline wont block hex.
            outline.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);        
        }
    }
    private void findHexGroup() //finds the hex group and adds them in the selectedGroup for outline
    {
        Vector2 firstHex, secondHex;
        
        selectedHexagon = allHexagons[XHex,YHex];
        otherTwoHex(out firstHex, out secondHex);
        if(selectedGroup!=null)
        {
            selectedGroup.Clear();
        }
        selectedGroup.Add(selectedHexagon);
        selectedGroup.Add(allHexagons[(int)firstHex.x,(int)firstHex.y]);
        selectedGroup.Add(allHexagons[(int)secondHex.x,(int)secondHex.y]);
    }
    private void otherTwoHex(out Vector2 firstHex, out Vector2 secondHex) //find the two hexes for selectedGroup
    {
        hexagon.otherHexes neighbours = selectedHexagon.GetComponent<hexagon>().getNeighbours();
        bool isFind = false;

        do
        {
            switch (SelectCount)  //other hexes depends on the SelectCount
            {
                case 0: firstHex = neighbours.up; secondHex = neighbours.upRight; break;
                case 1: firstHex = neighbours.upRight; secondHex = neighbours.downRight; break;
                case 2: firstHex = neighbours.downRight; secondHex = neighbours.down; break;
                case 3: firstHex = neighbours.down; secondHex = neighbours.downLeft; break;
                case 4: firstHex = neighbours.downLeft; secondHex = neighbours.upLeft; break;
                case 5: firstHex = neighbours.upLeft; secondHex = neighbours.up; break;
                default: firstHex = Vector2.zero; secondHex = Vector2.zero; break;
            }
          
            //if the hex is not available in the grid we increase the selectCount
            if (firstHex.x < 0 || firstHex.x >= GridWidth || firstHex.y < 0 || firstHex.y >= GridHeight || secondHex.x < 0 || secondHex.x >= GridWidth || secondHex.y < 0 || secondHex.y >= GridHeight)
            {
                SelectCount++;
                if (SelectCount >= 6)
                {
                    SelectCount = 0;
                }
            }
            else
            {
                isFind = true;
            }
        } while (!isFind);
    }
   


    public void Rotate(bool clockWise)
    {           
        if (canRotate == true)
        {
            canPlay = false;
            StartCoroutine(rotationCheck(clockWise));
        }
    }
    private IEnumerator rotationCheck(bool clockWise) // before full rotation, checks if there is a match after every position change 
    {
        canRotate = false;
        for (int i = 0; i < selectedGroup.Count; i++)
        {         
            swap(clockWise);
            yield return new WaitForSeconds(0.5f);
            find.findAllMatches();
            if (find.matches.Count > 0) //if there is a match stops rotating
            {
                destroyOutline();              
                break;
            }
        }    
        canRotate = true;
        canPlay = true;
        findHexGroup(); //find the group again for move
        checkBombs();
        if(matchesOnBoard()==true)
        {
            DestroyMatches();
        }
    }
    private void swap(bool clockWise) //changes every hex's x and y value in the grid and their positions
    {
        int[,] newPlace = new int[3, 2];

        Vector2 pos1, pos2, pos3;
        GameObject firstHex, secondHex, thirdHex;


        for (int i = 0; i < 3; i++)
        {
            newPlace[i, 0] = selectedGroup[i].GetComponent<hexagon>().x;
            newPlace[i, 1] = selectedGroup[i].GetComponent<hexagon>().y;
        }

        firstHex = selectedGroup[0];
        secondHex = selectedGroup[1];
        thirdHex = selectedGroup[2];

        pos1 = firstHex.transform.position;
        pos2 = secondHex.transform.position;
        pos3 = thirdHex.transform.position;



        if (clockWise)
        {
            firstHex.GetComponent<hexagon>().Rotate(newPlace[1, 0], newPlace[1, 1], pos2);
            allHexagons[newPlace[1, 0], newPlace[1, 1]] = firstHex;

            secondHex.GetComponent<hexagon>().Rotate(newPlace[2, 0], newPlace[2, 1], pos3);
            allHexagons[newPlace[2, 0], newPlace[2, 1]] = secondHex;

            thirdHex.GetComponent<hexagon>().Rotate(newPlace[0, 0], newPlace[0, 1], pos1);
            allHexagons[newPlace[0, 0], newPlace[0, 1]] = thirdHex;
        }
        else
        {
            firstHex.GetComponent<hexagon>().Rotate(newPlace[2, 0], newPlace[2, 1], pos3);
            allHexagons[newPlace[2, 0], newPlace[2, 1]] = firstHex;

            secondHex.GetComponent<hexagon>().Rotate(newPlace[0, 0], newPlace[0, 1], pos1);
            allHexagons[newPlace[0, 0], newPlace[0, 1]] = secondHex;

            thirdHex.GetComponent<hexagon>().Rotate(newPlace[1, 0], newPlace[1, 1], pos2);
            allHexagons[newPlace[1, 0], newPlace[1, 1]] = thirdHex;
        }
    }



    private void checkBombs() //checks for bombs in the grid and makes their countdown
    {
        for (int i = 0; i < GridWidth; i++)
        {
            for (int j = 0; j < GridHeight; j++)
            {
                if (allHexagons[i, j].GetComponent<hexagon>().isBomb == true)
                {
                    allHexagons[i, j].GetComponent<hexagon>().countDown();
                }
            }
        }
    }
    public bool isDouble(int x) //checks if the hex's x value in the grid is odd or even
    {
        return (0 == x % 2);
    }   
    public GameObject GetSelectedHexagon() //get function for selectedHexagon
    {
        return selectedHexagon;
    }
}
