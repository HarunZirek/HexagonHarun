using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class input : MonoBehaviour
{
    private bool isTouched;
    private hexagonGrid gridManager;
    private Vector2 startPos;
    private GameObject selectedHex;

    private void Start()
    {
        gridManager = FindObjectOfType<hexagonGrid>();
    }
    void Update()
    {
        //if there is a touch it gets touchs position and finds if it hits a collider
        if (Input.touchCount > 0)
        {
            Vector3 wp = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            Vector2 touchPos = new Vector2(wp.x, wp.y);
            Collider2D collider = Physics2D.OverlapPoint(touchPos);
            selectedHex = gridManager.GetSelectedHexagon();          
            touchDetection();
            CheckSelection(collider);
            CheckRotation();
        }
    } 


    private void touchDetection()
    {
        if (Input.GetTouch(0).phase == TouchPhase.Began)
        {
            isTouched = true;
            startPos = Input.GetTouch(0).position;
        }
    }

    //if the touch is just a tap and it hits a collider, we assign the touched collider to selected hex
    private void CheckSelection(Collider2D collider)
    {
        if (collider != null)
        {       
            if (Input.GetTouch(0).phase == TouchPhase.Ended && isTouched)
            {
                isTouched = false;
                gridManager.selectHex(collider);
            }
        }
    }

    //if the touch is a move, starts work
    private void CheckRotation()
    {
        if (Input.GetTouch(0).phase == TouchPhase.Moved && isTouched)
        {
            Vector2 currentPos = Input.GetTouch(0).position;
            float dX = currentPos.x - startPos.x;
            float dY = currentPos.y - startPos.y;


            //assign the move is clockwise or not by checking the direction of move and which side of the hexagon it is made.
            if ((Mathf.Abs(dX) > 5f || Mathf.Abs(dY) > 5f) && selectedHex != null)
            {
                Vector3 screenPos = Camera.main.WorldToScreenPoint(selectedHex.transform.position);
             
                bool triggerOnX = Mathf.Abs(dX) > Mathf.Abs(dY);
                bool swipeRightUp = triggerOnX ? dX > 0 : dY > 0;
                bool touchThanHex = triggerOnX ? currentPos.y > screenPos.y : currentPos.x > screenPos.x;
                bool clockWise = triggerOnX ? swipeRightUp == touchThanHex : swipeRightUp != touchThanHex;

                isTouched = false;

                //after finding the move and its direction calls the rotate function from grid class
                gridManager.Rotate(clockWise);
            }
        }
    }
}
