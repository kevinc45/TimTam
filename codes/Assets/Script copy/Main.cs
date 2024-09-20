using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    public Sprite bananaSprite;
    public Sprite chickenSprite;
    public Sprite cheeseSprite;
    public Sprite capsicumSprite;
    public Sprite beerSprite;


    public float totalTime = 30;
    public Text timerText;

    private string[] player1Task;
    private string[] player2Task;

    private Vector3[] player1RecipePosition = new Vector3[2];
    private Vector3[] player2RecipePosition = new Vector3[2];

    public float floatSpeed = 1.0f; // Speed of floating movement
    public float floatRangeX = 10.0f; // Horizontal range of floating movement
    public float floatRangeY = -10.0f; // Vertical range of floating movement


    private GameObject[] Player1Assigned;
    private GameObject[] Player2Assigned;


    // Start is called before the first frame update
    void Start()
    {
        player1RecipePosition[0] = new Vector3(-0.95f, 1.26999996f, -1f);
        player1RecipePosition[1] = new Vector3(-0.9f, -0.99f, -1f);
        player2RecipePosition[0] = new Vector3(1.03f, 1.23f, -1f);
        player2RecipePosition[1] = new Vector3(1.2200001f, -0.98f, -1f);

        Player1Assigned = new GameObject[4];
        Player2Assigned = new GameObject[4];
        // Time limitation for each round
        CountdownTime();

        // Assign recipe tasks to both players
        TaskAssign();
        IngredientAssign();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            P1KickIngredient();
        }
        // if (Input.GetKeyDown(KeyCode.I))
        // {
        //     P2KickIngredient();
        // }


    }

    // Generate random recipe tasks
    public string[] RecipeGenerator()
    {
        string[] ingredients = new string[] { "capsicum", "bananas", "cheese", "chicken", "beer" };
        int[] recipeNo = new int[2];
        string[] recipeTask = new string[2];

        for (int i = 0; i < 2; i++)
        {
            recipeNo[i] = UnityEngine.Random.Range(0, 5); // Randomly select 2 ingredients
        }

        for (int i = 0; i < recipeNo.Length; i++)
        {
            recipeTask[i] = ingredients[recipeNo[i]];
            Debug.Log(ingredients[recipeNo[i]]);
        }

        return recipeTask;
    }

    // Render the sprite for the ingredient
    // 19/20/2024: Changed return type to GameObject (public void to private GameObject)
    private GameObject SpriteRenderer(GameObject ingredient)
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
            case "beer":
                spriteRenderer.sprite = beerSprite;
                break;
        }
        return ingredient;
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

    // Ingredients assign
    public void IngredientAssign()
    {
        for (int i = 0; i < player1Task.Length; i++)
        {
            GameObject player1Ingredient = new GameObject();
            player1Ingredient.name = player1Task[i];
            player1Ingredient.transform.position = new Vector3(UnityEngine.Random.Range(2.5f, 4f), UnityEngine.Random.Range(-4f, 4f), -1);
            player1Ingredient.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            Player1Assigned[i] = SpriteRenderer(player1Ingredient); // Added on 19/09/2024
        }

        for (int i = 0; i < player2Task.Length; i++)
        {
            GameObject player2Ingredient = new GameObject();
            player2Ingredient.name = player2Task[i];
            player2Ingredient.transform.position = new Vector3(UnityEngine.Random.Range(-2.5f, -4f), UnityEngine.Random.Range(-4f, 4f), -1);
            player2Ingredient.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            Player2Assigned[i] = SpriteRenderer(player2Ingredient); // Added on 19/09/2024
        }
    }

    // Set countdown time
    public void CountdownTime()
    {
        if (totalTime > 0)
        {
            totalTime -= Time.deltaTime;  // Subtract elapsed time every frame
        }
        else
        {
            timerText.text = "Time's up";
            totalTime = 0;
        }
    }

    public void P1KickIngredient()
    {
        for (int i = 0; i < player1Task.Length; i++)
        {
            // GameObject[] player1Kicakble = Player1Assigned;
            // GameObject[] player2Kickable = Player2Assigned;

            if (Player1Assigned[i].name != player1Task[i])

            {
                Vector3 newPosition = new Vector3(
                    UnityEngine.Random.Range(2.5f, 4f),
                    UnityEngine.Random.Range(-4f, 4f),
                    -1
                );
                Player2Assigned[i].transform.position = newPosition;
            }
        }
    }

    // P2KickIngredeint will only work on 1 kickable ingredient for player 2 so far
    // TO DO: P2KickIngredient need to debug for multiple kickable ingredients
    // public void P2KickIngredient()
    // {
    //     for (int i = 0; i < player2Task.Length; i++)
    //     {
    //         GameObject[] player1Kicakble = Player2Assigned;
    //         GameObject[] player2Kickable = Player2Assigned;

    //         if (player2Kicakble.name != player2Task[i])
    //         {
    //             Vector3 newPosition = new Vector3(
    //                 UnityEngine.Random.Range(-2.5f, -4f),
    //                 UnityEngine.Random.Range(-4f, 4f),
    //                 -1
    //             );
    //             player1Kickable.transform.position = newPosition;
    //         }
    //     }
    // }

}
