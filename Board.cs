using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public static int widthOfBoard = 5;
    public static int heightOfBoard = 5;
    [SerializeField] float cellWidth = 1;
    [SerializeField] float cellHight = 1; 
    [SerializeField] GameObject myTile;
    public Sprite[] foodSprites;
    [SerializeField] float DelayBeforeSolveing = 1.5f; 
    private FruitCell fruitCellDown;
    private FruitCell fruitCellUp; 
    protected FruitCell[,] tiles;
    

    public void Start()
    {
        tiles = new FruitCell[widthOfBoard, heightOfBoard];
        LevelLoader();
        SolveGrid(); 
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            onMouseUp();
        }
        if(Input.GetMouseButtonDown(0))
        {
            onMouseDown(); 
        }


        
    }

    private void onMouseUp()
    {
       fruitCellUp = GetFruiteCellFromMousePos(Input.mousePosition);
        if (fruitCellUp == null)
        {
            return; 
        }
        if (fruitCellUp.IsNeiborCell(fruitCellDown.gridPos))
        {
            if (fruitCellDown == null)
            {
                return;
            }
            //swaps the foodtype. 
            FruitCell.FoodType temp = fruitCellDown.typeOfFood; 
            fruitCellDown.typeOfFood = fruitCellUp.typeOfFood;
            fruitCellUp.typeOfFood = temp;
            //swaps the sprites. 
            Sprite tempSprite = fruitCellDown.foodSprite.sprite;
            fruitCellDown.foodSprite.sprite = fruitCellUp.foodSprite.sprite;
            fruitCellUp.foodSprite.sprite = tempSprite;
            //sets the picked up fruit sprite back into it's cell. 
            fruitCellDown.foodSprite.gameObject.transform.localPosition = Vector3.zero;
        }
        else
        {
            fruitCellDown.foodSprite.gameObject.transform.localPosition = Vector3.zero; 
        }
        StartCoroutine(WaitAndSolveGrid()); 
         
    }

    private void onMouseDown()
    {
        fruitCellDown = GetFruiteCellFromMousePos(Input.mousePosition); 

    }

    private void LevelLoader()
    {
        for (int i = 0; i < widthOfBoard; i++)
        {
            for (int j = 0; j < heightOfBoard; j++)
            {
                //(Vector2)transform.position casts transform.position as a vector2 instead of vector3. 
                Vector2 position = (Vector2)transform.position + new Vector2(i * cellWidth, j * cellHight);
                GameObject tile = Instantiate(myTile, position, Quaternion.identity) as GameObject;
                tile.transform.parent = this.transform;
                FruitCell fruit = tile.GetComponent<FruitCell>();
                tiles[i, j] = fruit;
                fruit.gridPos = new Vector2(i, j); 
                tile.name = "(" + i + "," + j + ")";
                int whatTypeOfFood = Random.Range(0, foodSprites.Length);
                tiles[i, j].typeOfFood = (FruitCell.FoodType)whatTypeOfFood;
                tiles[i, j].foodSprite.sprite = foodSprites[whatTypeOfFood];
            }
        }
    }
    //this function solves the grid but looping using for loops to cheak every cell and cheak weather the cells beside or on the bottom or top of the cell match the cell. 
    private void SolveGrid()
    {

        Debug.Log("solveGrid"); 
        //two for loops used to cheak through every grid cell
        for (int i = 0; i < widthOfBoard; i++)
        {
            for (int j = 0; j < heightOfBoard; j++)
            {
                Debug.Log("cheaking cell " + i + "," + j); 
                FruitCell.FoodType tempFood = tiles[i, j].typeOfFood;
                FruitCell currentFruitCell = tiles[i, j]; 
                List<FruitCell> matchingCellsToTheRight = new List<FruitCell>();
                List<FruitCell> matchingCellsUp = new List<FruitCell>();
                FruitCell neiborFruitCell = currentFruitCell.getNeiborRight(tiles);
                //uses a while loop to cheak keep cheaking for matches as long as their is a neiboring Cell to the one being cheaked. 
                while (neiborFruitCell != null)
                {
                    //if statment used to cheak if cell being cheaked has a match with a horizontal cell. 
                    if (currentFruitCell.typeOfFood == neiborFruitCell.typeOfFood)
                    {
                        Debug.Log("H-match- " + neiborFruitCell.gridPos); 
                        matchingCellsToTheRight.Add(neiborFruitCell);
                        neiborFruitCell = neiborFruitCell.getNeiborRight(tiles);
                    }
                    //breaks out of the stament if their is no match. 
                    else
                    {
                        break;
                    }
                }
                neiborFruitCell = currentFruitCell.getNeiborUp(tiles);
                while (neiborFruitCell != null)
                {
                    if (currentFruitCell.typeOfFood == neiborFruitCell.typeOfFood)
                    {

                        //if statment used to cheak if cell being cheaked has a match with a vertical cell. 
                        Debug.Log("V-match- " + neiborFruitCell.gridPos);
                        matchingCellsUp.Add(neiborFruitCell);
                        neiborFruitCell = neiborFruitCell.getNeiborUp(tiles);
                    }
                    else
                    {
                        break;
                    }
                }
                //uses bools to tell game if the food in a cell needs to be replaced by another food when a match is found
                bool horizontalSolved = false;
                bool verticalSolved = false; 
                if(matchingCellsToTheRight.Count >= 2)
                {
                    horizontalSolved = true; 
                    //iterate over this list and replace the food type with a new food type. 
                    for(int b = 0; b < matchingCellsToTheRight.Count; b++)
                    {
                        FruitCell fruitCellToSolve = matchingCellsToTheRight[b];
                        Debug.Log("cells to the right match");
                        randomFoodSelection(fruitCellToSolve); 
                    }
                }
                if (matchingCellsUp.Count >= 2)
                {
                    verticalSolved = true; 
                    //iterate over this list and replace the food type with a new food type. 
                    for (int b = 0; b < matchingCellsUp.Count; b++)
                    {
                        FruitCell fruitCellToSolve = matchingCellsUp[b];
                        Debug.Log("cells up match");
                        randomFoodSelection(fruitCellToSolve);
                    }
                }
                //if we solved any fruit cells above or to the right thne break out of this 
                if(horizontalSolved || verticalSolved)
                {
                    //randomFoodSelection called to change food that matches. 
                    randomFoodSelection(currentFruitCell);
                    //resterts the solving prosses.
                    StartCoroutine(WaitAndSolveGrid()); 
                    break;
                }
            }
        }
    }
    //this function randomizes the typeOfFood and sprite that appears in the Fruitcell. 
    private void randomFoodSelection(FruitCell myCell)
    {
        int whatTypeOfFood = Random.Range(0, foodSprites.Length);
        myCell.typeOfFood = (FruitCell.FoodType)whatTypeOfFood;
        myCell.foodSprite.sprite = foodSprites[whatTypeOfFood]; 
    }

    private IEnumerator WaitAndSolveGrid()
    {
        yield return new WaitForSeconds(DelayBeforeSolveing);
        SolveGrid(); 
    }
    private FruitCell GetFruiteCellFromMousePos(Vector2 _mousePos)
    {
        Debug.Log("mousex - " + _mousePos.x + " mouse y - " + _mousePos.y);
        Vector2 startPos = transform.position;
        float cellXhalf = cellWidth / 2f;
        float CellYhalf = cellHight / 2f;
        //this converts my mouse from screen position to world position. 
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(_mousePos); 
        Vector2 nomalizedMousePos = mouseWorldPos - startPos;
        //normalizes the mouse position in unity to my grid position. 
        int mouseXGridPos = (int)Mathf.Ceil((nomalizedMousePos.x - cellXhalf) / cellWidth);
        int mouseYGridPos = (int)Mathf.Ceil((nomalizedMousePos.y - CellYhalf) / cellHight);
       // Debug.Log("mouseGridPos" + " x " + mouseXGridPos + " y " + mouseYGridPos);
        if (mouseXGridPos > 5 || mouseXGridPos < 0 || mouseYGridPos > 5 || mouseYGridPos < 0)
        {
            return null; 
        }
        return tiles[mouseXGridPos, mouseYGridPos]; 
    }
    
}
