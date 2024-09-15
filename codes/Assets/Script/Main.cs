using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    public Sprite bananaSprite;
    public Sprite chickenSprite;
    public Sprite cheeseSprite;
    public Sprite capsicumSprite;

    private string[] player1Task;
    private string[] player2Task;

    private Vector3[] player1RecipePosition = new Vector3[2];
    private Vector3[] player2RecipePosition = new Vector3[2];
    
    // Start is called before the first frame update
    void Start()
    {
        // Define positions for both players
        player1RecipePosition[0] = new Vector3(-0.95f, 1.26999996f, -1f);
        player1RecipePosition[1] = new Vector3(1.03f, 1.23f, -1f);
        player2RecipePosition[0] = new Vector3(-0.9f, -0.99f, -1f);
        player2RecipePosition[1] = new Vector3(1.2200001f, -0.98f, -1f);
        
        // Assign recipe tasks to both players
        TaskAssign();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    // Generate random recipe tasks
    public string[] RecipeGenerator()
    {
        string[] ingredients = new string[] { "capsicum", "bananas", "cheese", "chicken" };
        int[] recipeNo = new int[2];
        string[] recipeTask = new string[2];
        
        for (int i = 0; i < 2; i++)
        {
            recipeNo[i] = UnityEngine.Random.Range(0, 4); // Randomly select 2 ingredients
        }

        for (int i = 0; i < recipeNo.Length; i++)
        {
            recipeTask[i] = ingredients[recipeNo[i]];
            Debug.Log(ingredients[recipeNo[i]]);
        }

        return recipeTask;
    }

    // Render the sprite for the ingredient
    public void SpriteRenderer(GameObject ingredient)
    {
        SpriteRenderer spriteRenderer = ingredient.AddComponent<SpriteRenderer>();
        switch (ingredient.name)
        {
            case "bananas":
                spriteRenderer.sprite = bananaSprite;
                break;
            case "chicken":
                spriteRenderer.sprite = chickenSprite;
                break;
            case "cheese":
                spriteRenderer.sprite = cheeseSprite;
                break;
            case "capsicum":
                spriteRenderer.sprite = capsicumSprite;
                break;
        }
    }
    
    // Assign tasks and positions for both players
    public void TaskAssign()
    {
        player1Task = RecipeGenerator();
        player2Task = RecipeGenerator();

        // Create and place objects for player 1's tasks
        for (int i = 0; i < player1Task.Length; i++)
        {
            GameObject ingredient = new GameObject(player1Task[i]);
            SpriteRenderer(ingredient);
            SetPosition(ingredient, player1RecipePosition[i]); // Assign specific position
        }

        // Create and place objects for player 2's tasks
        for (int i = 0; i < player2Task.Length; i++)
        {
            GameObject ingredient = new GameObject(player2Task[i]);
            SpriteRenderer(ingredient);
            SetPosition(ingredient, player2RecipePosition[i]); // Assign specific position
        }

        Debug.Log("Player 1 Tasks: " + string.Join(", ", player1Task));
        Debug.Log("Player 2 Tasks: " + string.Join(", ", player2Task));
    }

    // Set position and scale for each ingredient
    public void SetPosition(GameObject ingredient, Vector3 position)
    {
        ingredient.transform.position = position;
        ingredient.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f); // Uniform scaling
    }
}
