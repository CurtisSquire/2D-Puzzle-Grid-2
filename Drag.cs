using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag : MonoBehaviour
{
    //prevent player from changing food while grid is solving. 
    protected Vector2 thisObjectsPos;
    public SpriteRenderer thisSprite;
    string SpriteName; 

    protected bool hasColWithFruit = false;
    private void Start()
    {
        thisObjectsPos = transform.position; 
    }

 void ReturnToPos()
    {
        transform.position = thisObjectsPos; 
    }

    private void Update()
    {
        if (hasColWithFruit)
        {
            
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "OtherFruit")
        {
            Debug.Log("entered"); 
            SpriteName = col.gameObject.GetComponent<SpriteRenderer>().sprite.name; 
        }
    }

    private void OnCollisionExit2D(Collision2D c)
    {
        if(c.gameObject.tag == "OtherFruit")
        {
            Debug.Log("exited"); 
            thisSprite = GetComponent<SpriteRenderer>(); 
            thisSprite.sprite = (Sprite)Resources.Load<Sprite>("Sprites/" + SpriteName) as Sprite; 
        }
    }

    float distance = 10;
    private void OnMouseDrag()
    {
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
        Vector3 objectPos = Camera.main.ScreenToWorldPoint(mousePos);
        transform.position = objectPos;
        thisSprite.sortingOrder = 21; 
    }
    private void OnMouseUp()
    {
        thisSprite.sortingOrder = 2; 
    }


}
