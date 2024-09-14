using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    public Sprite bananaSprite;
    public Sprite chickenSprite;
    public Sprite cheeseSprite;
    public Sprite capsicumSprite;

    // Start is called before the first frame update
    void Start()
    {
        DisplayRecipe();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    // Setup recipe task
    public string[] DisplayRecipe()
    {
        string[] ingredients = new string[] { "capsicum", "bananas", "cheese", "chicken" };
        string[] recipeTask = new string[2];
        int[] recipeNo = new int[2];

        for (int i = 0; i < 2; i++)
        {
            recipeNo[i] = UnityEngine.Random.Range(0, 4); // Use 0 to 3 for all ingredients
        }

        for (int i = 0; i < recipeNo.Length; i++)
        {
            recipeTask[i] = ingredients[recipeNo[i]];
            Debug.Log(ingredients[recipeNo[i]]);
        }
        
        for (int i = 0; i < recipeNo.Length; i++)
        {
            GameObject ingredient = new GameObject(recipeTask[i]);
            SpriteRenderer(ingredient);
        }

        return recipeTask;
    }

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
}
