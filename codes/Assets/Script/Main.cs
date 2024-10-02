using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.IO.Ports;

public class Main : MonoBehaviour
{
    public Sprite bananaSprite;
    public Sprite chickenSprite;
    public Sprite cheeseSprite;
    public Sprite capsicumSprite;
    public Sprite beerSprite;
    public Sprite yummySprite;

    private bool isPaused = false;
    public float totalTime = 10;

    private string[] player1Task;
    private string[] player2Task;

    private Vector3[] player1RecipePosition = new Vector3[2];
    private Vector3[] player2RecipePosition = new Vector3[2];

    private List<GameObject> player1Assigned = new List<GameObject>();
    private List<GameObject> player2Assigned = new List<GameObject>();

    private bool complete = false;
    public GameObject player1Yummy;
    public GameObject player2Yummy;

    // Arduino connection    
    private SerialPort serialPort1;
    private SerialPort serialPort2;
    public string portName1 = "COM16"; // Change this depend on the FIRST Arduino's port
    public string portName2 = "COM19"; // Change this depend on the SECOND Arduino's port
    public int baudRate = 9600;

    // Start is called before the first frame update
    void Start()
    {
        player1RecipePosition[0] = new Vector3(-0.95f, 1.26999996f, -1f);
        player1RecipePosition[1] = new Vector3(-0.9f, -0.99f, -1f);
        player2RecipePosition[0] = new Vector3(1.03f, 1.23f, -1f);
        player2RecipePosition[1] = new Vector3(1.2200001f, -0.98f, -1f);

        // Time limitation for each round
        CountdownTime();

        // Assign recipe tasks to both players
        TaskAssign();
        IngredientAssign();

        // Initiating Arduino Connection
        serialPort1 = new SerialPort(portName1, baudRate);
        serialPort2 = new SerialPort(portName2, baudRate);

        if (!serialPort1.IsOpen)
        {
            serialPort1.Open();
            serialPort1.ReadTimeout = 1000;
        }
        if (!serialPort2.IsOpen)
        {
            serialPort2.Open();
            serialPort2.ReadTimeout = 1000;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isPaused) return;

        // if (serialPort != null)
        // {
        //     Debug.Log("Serial Port is not null");
        // }
        // if (serialPort.IsOpen)
        // {
        //     Debug.Log("Serial Port is Open");
        // }
        // if (serialPort.BytesToRead > 0)
        // {
        //     Debug.Log("BytesToRead > 0");
        // }
        
        // Arduino 1, Player 1
        if (serialPort1 != null && serialPort1.IsOpen && serialPort1.BytesToRead > 0)
        {
            string data1 = serialPort1.ReadLine();
            string[] distances1 = data1.Split(',');

            if (distances1.Length == 4)
            {
                if (float.TryParse(distances1[0], out float player1Distance1) &&
                    float.TryParse(distances1[1], out float player1Distance2) &&
                    float.TryParse(distances1[2], out float player1Distance3) &&
                    float.TryParse(distances1[3], out float player1Distance4))
                {
                    if (player1Distance1 < 20){
                        P1KickIngredient(0);
                    }
                    else if (player1Distance2 < 20){
                        P1KickIngredient(1);
                    }
                    else if (player1Distance3 < 20){
                        P1KickIngredient(2);
                    }
                    else if (player1Distance4 < 20){
                        P1KickIngredient(3);
                    }
                }
            }
        }

        // Arduino 2, Player 2
        if (serialPort2 != null && serialPort2.IsOpen && serialPort2.BytesToRead > 0)
        {
            string data2 = serialPort2.ReadLine();
            string[] distances2 = data2.Split(',');

            if (distances2.Length == 4)
            {
                if (float.TryParse(distances2[0], out float player2Distance1) &&
                    float.TryParse(distances2[1], out float player2Distance2) &&
                    float.TryParse(distances2[2], out float player2Distance3) &&
                    float.TryParse(distances2[3], out float player2Distance4))
                {
                    if (player2Distance1 < 20){
                        P2KickIngredient(0);
                    }
                    else if (player2Distance2 < 20){
                        P2KickIngredient(1);
                    }
                    else if (player2Distance3 < 20){
                        P2KickIngredient(2);
                    }
                    else if (player2Distance4 < 20){
                        P2KickIngredient(3);
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            P1KickIngredient(0);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            P1KickIngredient(1);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            P1KickIngredient(2);
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            P1KickIngredient(3);
        }
        else if (Input.GetKeyDown(KeyCode.U))
        {
            P2KickIngredient(0);
        }
        else if (Input.GetKeyDown(KeyCode.I))
        {
            P2KickIngredient(1);
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            P2KickIngredient(2);
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            P2KickIngredient(3);
        }

        if (totalTime > 0)
        {
            CountdownTime();
            if (CheckComplete())
            {
                player1Yummy.SetActive(true);
                player2Yummy.SetActive(true);
                PauseForTwoSeconds();
            }
        }
        else
        {
            Debug.Break();
        }
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
            case "yummy":
                spriteRenderer.sprite = yummySprite;
                break;
        }
        return ingredient;
    }

    // Assign tasks and positions for both players
    public void TaskAssign()
    {
        player1Task = RecipeGenerator();
        player2Task = RecipeGenerator();

        GameObject player1Parent = GameObject.Find("Player1Recipe");
        GameObject player2Parent = GameObject.Find("Player2Recipe");
        // Create and place objects for player 1's tasks
        for (int i = 0; i < player1Task.Length; i++)
        {
            GameObject ingredient = new GameObject(player1Task[i]);
            SpriteRenderer(ingredient);
            SetPosition(ingredient, player1RecipePosition[i]); // Assign specific position
            ingredient.transform.SetParent(player1Parent.transform);
        }

        // Create and place objects for player 2's tasks
        for (int i = 0; i < player2Task.Length; i++)
        {
            GameObject ingredient = new GameObject(player2Task[i]);
            SpriteRenderer(ingredient);
            SetPosition(ingredient, player2RecipePosition[i]); // Assign specific position
            ingredient.transform.SetParent(player2Parent.transform);
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
        GameObject player1Parent = GameObject.Find("Player1Ingredients");
        GameObject player2Parent = GameObject.Find("Player2Ingredients");

        for (int i = 0; i < player1Task.Length; i++)
        {
            GameObject player1Ingredient = new GameObject();
            player1Ingredient.name = player1Task[i];
            Vector3 p1Position;
            bool positionValid;

            do
            {
                positionValid = true;
                float randomX = Mathf.Round(UnityEngine.Random.Range(2.5f, 4f) * 10) / 10;
                float randomY = Mathf.Round(UnityEngine.Random.Range(-4f, 4f) * 10) / 10;
                p1Position = new Vector3(randomX, randomY, -1);

                foreach (var ingredient in player2Assigned)
                {
                    Debug.Log("Distance 2: " + Vector3.Distance(p1Position, ingredient.transform.position));
                    if (Vector3.Distance(p1Position, ingredient.transform.position) < 3.0f)
                    {
                        positionValid = false;
                        break;
                    }
                }
            } while (!positionValid);

            player1Ingredient.transform.position = p1Position;
            // player1Ingredient.transform.position = new Vector3(UnityEngine.Random.Range(2.5f, 4f), UnityEngine.Random.Range(-4f, 4f), -1);
            player1Ingredient.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            player2Assigned.Add(SpriteRenderer(player1Ingredient));  // change the player1Assigned to list
            player1Ingredient.transform.SetParent(player1Parent.transform);
        }
        SortByYAxis(player2Assigned);

        for (int i = 0; i < player2Task.Length; i++)
        {
            GameObject player2Ingredient = new GameObject();
            player2Ingredient.name = player2Task[i];
            Vector3 p2Position;
            bool positionValid;

            do
            {
                positionValid = true;
                float randomX = Mathf.Round(UnityEngine.Random.Range(-2.5f, -4f) * 10) / 10;
                float randomY = Mathf.Round(UnityEngine.Random.Range(-4f, 4f) * 10) / 10;
                p2Position = new Vector3(randomX, randomY, -1);

                foreach (var ingredient in player1Assigned)
                {
                    Debug.Log("Distance 1: " + Vector3.Distance(p2Position, ingredient.transform.position));
                    if (Vector3.Distance(p2Position, ingredient.transform.position) < 3.0f)
                    {
                        positionValid = false;
                        break;
                    }
                }
            } while (!positionValid);

            player2Ingredient.transform.position = p2Position;
            // player2Ingredient.transform.position = new Vector3(UnityEngine.Random.Range(-2.5f, -4f), UnityEngine.Random.Range(-4f, 4f), -1);
            player2Ingredient.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            player1Assigned.Add(SpriteRenderer(player2Ingredient));  // change the player2Assigned to list
            player2Ingredient.transform.SetParent(player2Parent.transform);
        }
        SortByYAxis(player1Assigned);
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
            totalTime = 0;
        }
    }

    public void P1KickIngredient(int index)
    {
        if (index >= 0 && index < player1Assigned.Count)
        {
            float originalPositionX = player1Assigned[index].transform.position.x;
            float originalPositionY = player1Assigned[index].transform.position.y;
            player2Assigned.Add(player1Assigned[index]);
            player1Assigned.RemoveAt(index);

            // Vector3 newPosition = new Vector3(UnityEngine.Random.Range(2.5f, 4f), UnityEngine.Random.Range(-4f, 4f), -1);
            if (player2Assigned.Count == 0)
            {
                // player2Assigned[0].transform.position = newPosition;
                player2Assigned[0].transform.position = new Vector3(-originalPositionX, originalPositionY, -1);
            }
            else
            {
                // player2Assigned[player2Assigned.Count() - 1].transform.position = newPosition;
                player2Assigned[player2Assigned.Count() - 1].transform.position = new Vector3(-originalPositionX, originalPositionY, -1);
            }

            SortByYAxis(player1Assigned);
            SortByYAxis(player2Assigned);

            // Log the sorted lists
            Debug.Log("Player 1 Assigned after sorting by Y-axis:");
            for (int i = 0; i < player1Assigned.Count; i++)
            {
                Debug.Log("Player 1 Assigned: " + player1Assigned[i].name + " Y: " + player1Assigned[i].transform.position.y);
            }

            Debug.Log("Player 2 Assigned after sorting by Y-axis:");
            for (int i = 0; i < player2Assigned.Count; i++)
            {
                Debug.Log("Player 2 Assigned: " + player2Assigned[i].name + " Y: " + player2Assigned[i].transform.position.y);
            }
        }
    }

    public void P2KickIngredient(int index)
    {
        if (index >= 0 && index < player2Assigned.Count)
        {
            float originalPositionX = player2Assigned[index].transform.position.x;
            float originalPositionY = player2Assigned[index].transform.position.y;
            player1Assigned.Add(player2Assigned[index]);
            player2Assigned.RemoveAt(index);
            // Vector3 newPosition = new Vector3(UnityEngine.Random.Range(-2.5f, -4f), UnityEngine.Random.Range(-4f, 4f), -1);
            if (player1Assigned.Count == 0)
            {
                // player1Assigned[0].transform.position = newPosition;
                player1Assigned[0].transform.position = new Vector3(-originalPositionX, originalPositionY, -1);
            }
            else
            {
                // player1Assigned[player1Assigned.Count() - 1].transform.position = newPosition;
                player1Assigned[player1Assigned.Count() - 1].transform.position = new Vector3(-originalPositionX, originalPositionY, -1);
            }

            SortByYAxis(player1Assigned);
            SortByYAxis(player2Assigned);

            // Log the sorted lists
            Debug.Log("Player 1 Assigned after sorting by Y-axis:");
            for (int i = 0; i < player1Assigned.Count; i++)
            {
                Debug.Log("Player 1 Assigned: " + player1Assigned[i].name + " Y: " + player1Assigned[i].transform.position.y);
            }

            Debug.Log("Player 2 Assigned after sorting by Y-axis:");
            for (int i = 0; i < player2Assigned.Count; i++)
            {
                Debug.Log("Player 2 Assigned: " + player2Assigned[i].name + " Y: " + player2Assigned[i].transform.position.y);
            }
        }
    }

    public void InitialRound()
    {
        player1Assigned.Clear();
        player2Assigned.Clear();
        Array.Clear(player1Task, 0, player1Task.Length);
        Array.Clear(player2Task, 0, player1Task.Length);
    }

    private void SortByYAxis(List<GameObject> assignedList)
    {
        assignedList.Sort((a, b) => b.transform.position.y.CompareTo(a.transform.position.y));  // change the order of the ingredients based on position
    }

    public bool CheckComplete()
    {
        // Check if both players have exactly 2 assigned items
        if (player1Assigned.Count != 2 || player2Assigned.Count != 2)
        {
            return false; // Not complete if either player does not have 2 items
        }

        // Check player 1's assigned items against their tasks
        foreach (var task in player1Task)
        {
            // Check if the assigned items match the tasks
            if (!player1Assigned.Any(ingredient => ingredient.name == task))
            {
                return false; // If any task is not matched, return false
            }
        }

        // Check player 2's assigned items against their tasks
        foreach (var task in player2Task)
        {
            // Check if the assigned items match the tasks
            if (!player2Assigned.Any(ingredient => ingredient.name == task))
            {
                return false; // If any task is not matched, return false
            }
        }

        return true; // All tasks matched
    }

    public void DestoryGameObject()
    {
        foreach (Transform child in GameObject.Find("Player1Recipe").transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in GameObject.Find("Player2Recipe").transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in GameObject.Find("Player1Ingredients").transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in GameObject.Find("Player2Ingredients").transform)
        {
            Destroy(child.gameObject);
        }

        player1Yummy.SetActive(false);
        player2Yummy.SetActive(false);
    }

    public void PauseForTwoSeconds()
    {
        isPaused = true; // Set the pause flag
        StartCoroutine(PauseCoroutine());
    }

    private IEnumerator PauseCoroutine()
    {
        Debug.Log("Pausing game...");
        Time.timeScale = 0; // Pause the game
        yield return new WaitForSecondsRealtime(2); // Wait for 1 real seconds
        Time.timeScale = 1; // Resume the game
        Debug.Log("Resuming game...");
        DestoryGameObject();
        InitialRound();
        TaskAssign();
        IngredientAssign();

        isPaused = false; // Reset the pause flag
    }

    private void OnApplicationQuit()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
        }
    }
}