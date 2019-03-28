using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitCell : MonoBehaviour
{
    public SpriteRenderer foodSprite; 
    public Vector2 gridPos;
    public FoodType typeOfFood = FoodType.Undefined;
    private int changeToThisFoodType = 0; 
    public enum FoodType
    {
        Undefined = -1, 
        Apple = 0, 
        Bacon = 1, 
        Brownie = 2, 
        Cheese = 3, 
        Chicken = 4, 
        Eggs = 5, 
        Honey = 6, 
        Jam =7
    } 

    public void ChangeFruit()
    {
        //this is where i switch the image and change the food type. 
        //GetComponent<>
        changeToThisFoodType = Random.Range(0, 7);
        typeOfFood = (FoodType)changeToThisFoodType;
    }

    public bool IsNeiborCell(Vector2 _gridPos)
    {
        if (gridPos.x == _gridPos.x || gridPos.y == _gridPos.y)
        {
            return true; 
        }
        return false; 
    }

    //Here is where i cheak the food that is contained in the nieboring cells. 
    public FruitCell getNeiborRight(FruitCell[,] grid)
    {
        //find the grid position to the right by incrementing x if it is less then the width of the board.
        if (gridPos.x + 1 < Board.widthOfBoard)
        {
            return grid[(int)gridPos.x + 1, (int)gridPos.y];
        }
        else
        {
            return null; 
        }
         
    }

    public FruitCell getNeiborUp(FruitCell[,] grid)
    {
        //find the grid position up by incrementing y if it is less then the hieght of the board.
        if (gridPos.y + 1 < Board.heightOfBoard)
        {
            return grid[(int)gridPos.x, (int)gridPos.y + 1];
        }
        else
        {
            return null;
        }
    }


}
