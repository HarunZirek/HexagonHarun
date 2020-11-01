using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class hexagon : MonoBehaviour
{
    public TextMeshProUGUI countDownText;
    public GameObject countDownShow;
    public int x;
    public int y;

    private Vector2 lerpPosition;
    private bool lerp;

    public bool isMatched = false;
    public bool isBomb = false;
    private int countDownNumber = 8;

    hexagonGrid GridManager;
    private void Start()
    {
        GridManager = FindObjectOfType<hexagonGrid>();
    }
    void Update()
    {
        //every hex has the bomb option and has a canvas for this. If the hex is a bomb, turn on the canvas for countdown. By default it is off from editor in unity and changable easily
        if (isBomb==true)  
        {
            countDownShow.SetActive(true);
            countDownText.text = countDownNumber.ToString();
        }

        //if lerp is true rotates the function. the lerp is controlled by rotate function
        if (lerp)
        {
            float newX = Mathf.Lerp(transform.position.x, lerpPosition.x, Time.deltaTime * 9);
            float newY = Mathf.Lerp(transform.position.y, lerpPosition.y, Time.deltaTime * 9);
            transform.position = new Vector2(newX, newY);


            if (Vector3.Distance(transform.position, lerpPosition) < 0.05f)
            {
                transform.position = lerpPosition;
                lerp = false;
            }
        }
    }

    //variables for this hex's neighbours
    public struct otherHexes
    {
        public Vector2 up;
        public Vector2 upLeft;
        public Vector2 upRight;
        public Vector2 down;
        public Vector2 downLeft;
        public Vector2 downRight;
    }

    //for finding this hexagon neighbours. We check the X value for this. Because some neighbours's position depends on X value
    public otherHexes getNeighbours()
    {
        otherHexes neighbours;
        bool isDown = GridManager.isDouble(x);

        neighbours.down = new Vector2(x, y - 1);
        neighbours.up = new Vector2(x, y + 1);
        neighbours.upLeft = new Vector2(x - 1, !isDown ? y + 1 : y);
        neighbours.upRight = new Vector2(x + 1, !isDown ? y + 1 : y);
        neighbours.downLeft = new Vector2(x - 1, !isDown ? y : y - 1);
        neighbours.downRight = new Vector2(x + 1, !isDown ? y : y - 1);


        return neighbours;
    }

    /*the function for rotating hex to other hex's position (for gameplay this function will be used for 3 hexes in every move)
    this function not rotates just tells the update the rotate the hex */
    public void Rotate(int newX, int newY, Vector2 newPos)
    {
        lerpPosition = newPos;
        x = newX;
        y = newY;
        lerp = true;
    }

    //the function for moving hexes instead of teleporting them after generating new hexes or moving current ones into empty places
    public void changeToNewPos(Vector2 newPos)
    {
        lerpPosition = newPos;
        lerp = true;
    }

    // a countdown function for bomb hexagons, if the counter reaches zero makes isGameOver value true
    public void countDown()
    {
        countDownNumber--;
        if(countDownNumber <= 0)
        {
            GridManager.isGameOver = true;
        }
    }
}
