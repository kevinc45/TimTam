using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO.Ports;
using TMPro;

public class Main : MonoBehaviour
{
    // Sprites
    public Sprite bananaSprite;
    public Sprite chickenSprite;
    public Sprite cheeseSprite;
    public Sprite capsicumSprite;
    public Sprite beerSprite;
    public Sprite yummySprite;
    public Sprite redLightSprite;
    public Sprite yellowLightSprite;
    public Sprite greenLightSprite;
    public Sprite startSprite;
    public Sprite timesUpSprite;
    public Sprite pinkIngredientBgSprite;
    public Sprite greenIngredientBgSprite;
    public Sprite whiteIngredientBgSprite;
    public Sprite beigeIngredientBgSprite;
    
    // Audio
    public AudioSource yummyAudio;
    
    // Time setting
    private bool isPaused = false;
    public float totalTime;
    public TextMeshProUGUI player1CountDown;
    public TextMeshProUGUI player2CountDown;

    // Players' recipes
    private string[] player1Task;
    private string[] player2Task;
    private Vector3[] player1RecipePosition = new Vector3[2];
    private Vector3[] player2RecipePosition = new Vector3[2];

    // Players' ingredients
    private List<GameObject> player1Assigned = new List<GameObject>();
    private List<GameObject> player2Assigned = new List<GameObject>();
    
    // Finish recipe
    private bool complete = false;
    public GameObject player1Yummy;
    public GameObject player2Yummy;
    
    // Arduino connection    
    private SerialPort serialPort1;
    private SerialPort serialPort2;
    public string portName1 = "COM16"; // Change this depend on the FIRST Arduino's port
    public string portName2 = "COM19"; // Change this depend on the SECOND Arduino's port
    public int baudRate = 9600;
    private bool isArduinoConnected = false;

    void Start()
    {
        // Setting up players' 4 recipe position
        player1RecipePosition[0] = new Vector3(1.3f, -1.6f, -2f);
        player1RecipePosition[1] = new Vector3(1.3f, 1f, -2f);
        player2RecipePosition[0] = new Vector3(-2f, -1.6f, -2f);
        player2RecipePosition[1] = new Vector3(-2f, 1f, -2f);

        // Start countdown screen
        String[] startLight = {"red", "yellow", "green", "start"};
        StartLight(startLight);

        // Assign recipe tasks to both players (initial round)
        TaskAssign();
        IngredientAssign();  

        try{
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
            isArduinoConnected = true;
        }
        catch{
            Debug.Log("Arduino is not connected");
            isArduinoConnected = false;
        }
    }

    void Update()
    {
        if (isPaused) return;  // Wait 2 secs after showing yummy yummy

        if (isArduinoConnected){
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
                        if (player1Distance1 < 50){
                            P1KickIngredient(0);
                        }
                        else if (player1Distance2 < 50){
                            P1KickIngredient(1);
                        }
                        else if (player1Distance3 < 50){
                            P1KickIngredient(2);
                        }
                        else if (player1Distance4 < 50){
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
                        if (player2Distance1 < 50){
                            P2KickIngredient(0);
                        }
                        else if (player2Distance2 < 50){
                            P2KickIngredient(1);
                        }
                        else if (player2Distance3 < 50){
                            P2KickIngredient(2);
                        }
                        else if (player2Distance4 < 50){
                            P2KickIngredient(3);
                        }
                    }
                }
            }
        }

        // Except arduino, keyboard can also control the kicking ingredient action
        // Read ingredients from left to right, both players will have min 0 ingredients to max 4 ingredients
        // Key: QWER > control player 1's ingredients from left to right with QWER respectively (left half)
        // Key: UIOP > control player 2's ingredients from left to right with UIOP respectively(right half)
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

        // Countdown timer, timer is based on the traffic light, for prototype set for 63 secs
        if (totalTime > 0)
        {
            CountdownTime();
            // Check if the ingredients match the recipe for both players
            if (CheckComplete())
            {
                yummyAudio.Play();
                player1Yummy.SetActive(true);
                player2Yummy.SetActive(true);
                PauseForTwoSeconds();
            }
        }
        else
        {
            TimesUp();
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
            recipeNo[i] = UnityEngine.Random.Range(0, 5);  // Randomly select 2 ingredients
        }

        for (int i = 0; i < recipeNo.Length; i++)
        {
            recipeTask[i] = ingredients[recipeNo[i]];
        }

        return recipeTask;
    }

    // Set sprite for each GameObject
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
            case "red":
                spriteRenderer.sprite = redLightSprite;
                break;
            case "yellow":
                spriteRenderer.sprite = yellowLightSprite;
                break;
            case "green":
                spriteRenderer.sprite = greenLightSprite;
                break;
            case "start":
                spriteRenderer.sprite = startSprite;
                break;
            case "timesUp":
                spriteRenderer.sprite = timesUpSprite;
                break;
            case "pinkBg":
                spriteRenderer.sprite = pinkIngredientBgSprite;
                break;
            case "whiteBg":
                spriteRenderer.sprite = whiteIngredientBgSprite;
                break;
            case "greenBg":
                spriteRenderer.sprite = greenIngredientBgSprite;
                break;
            case "beigeBg":
                spriteRenderer.sprite = beigeIngredientBgSprite;
                break;
        }
        return ingredient;
    }

    // Assign tasks and positions for both players
    // For prototype, exchage the recipe as their ingredients
    public void TaskAssign()
    {
        bool isTaskSame = false;  // Check if tasks of two players are the same
        
        // Avoid matched ingredients and recipe when refreshing
        do
        {
            player1Task = RecipeGenerator();
            player2Task = RecipeGenerator();
            
            if (player1Task != null && player2Task != null && player1Task.Length == player2Task.Length)
            {
                HashSet<string> player1TaskSet = new HashSet<string>(player1Task);  // Convert arrays to HashSet to ignore the order of tasks
                HashSet<string> player2TaskSet = new HashSet<string>(player2Task);
                
                isTaskSame = player1TaskSet.SetEquals(player2TaskSet);  // Check if both sets are equal

                if (isTaskSame)
                {
                    Debug.Log("Same task, regenerate the task!");
                }
                else
                {
                    Debug.Log("Different task, do not regenerate the task!");
                }
            }
        } while (isTaskSame);

        
        GameObject player1Parent = GameObject.Find("Player1Recipe");
        GameObject player2Parent = GameObject.Find("Player2Recipe");
        
        // Create and place objects for player 1's tasks
        for (int i = 0; i < player1Task.Length; i++)
        {
            GameObject ingredient = new GameObject(player1Task[i]);
            ingredient.transform.SetParent(player1Parent.transform);
            SpriteRenderer(ingredient);
            SetPosition(ingredient, player1RecipePosition[i]);  // Assign specific position
        }

        // Create and place objects for player 2's tasks
        for (int i = 0; i < player2Task.Length; i++)
        {
            GameObject ingredient = new GameObject(player2Task[i]);
            ingredient.transform.SetParent(player2Parent.transform);
            SpriteRenderer(ingredient);
            SetPosition(ingredient, player2RecipePosition[i]);  // Assign specific position
        }
    }

    // Set position and scale for each ingredient
    public void SetPosition(GameObject ingredient, Vector3 position)
    {
        ingredient.transform.localPosition = position;
        ingredient.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
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
            
            // Check if the ingredients position is overlapping
            do
            {
                positionValid = true;
                float randomX = Mathf.Round(UnityEngine.Random.Range(2.8f, 7.5f) * 10) / 10;
                float randomY = Mathf.Round(UnityEngine.Random.Range(-4f, 4f) * 10) / 10;
                p1Position = new Vector3(randomX, randomY, -1f);
            
                foreach (var ingredient in player2Assigned)
                {
                    if (Vector3.Distance(p1Position, ingredient.transform.position) < 3.0f)
                    {
                        positionValid = false;
                        break;
                    }
                }
            } while (!positionValid);
            
            player1Ingredient.transform.SetParent(player1Parent.transform);
            player1Ingredient.transform.localPosition = p1Position;
            player1Ingredient.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            player2Assigned.Add(SpriteRenderer(player1Ingredient));  // Change the player1Assigned to list
            
        }
        SortByYAxis(player2Assigned);  // Reorder player's ingredients order according to its position

        for (int i = 0; i < player2Task.Length; i++)
        {
            GameObject player2Ingredient = new GameObject();
            player2Ingredient.name = player2Task[i];
            Vector3 p2Position;
            bool positionValid;
            
            do
            {
                positionValid = true;
                float randomX = Mathf.Round(UnityEngine.Random.Range(-3f, -7.5f) * 10) / 10;
                float randomY = Mathf.Round(UnityEngine.Random.Range(-4f, 4f) * 10) / 10;
                p2Position = new Vector3(randomX, randomY, -1f);
            
                foreach (var ingredient in player1Assigned)
                {
                    if (Vector3.Distance(p2Position, ingredient.transform.position) < 3.0f)
                    {
                        Debug.Log("not short");
                        positionValid = false;
                        break;
                    }
                }
            } while (!positionValid);

            player2Ingredient.transform.SetParent(player2Parent.transform);
            player2Ingredient.transform.localPosition = p2Position;
            player2Ingredient.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            player1Assigned.Add(SpriteRenderer(player2Ingredient));  // Change the player2Assigned to list
        }
        SortByYAxis(player1Assigned);
        AddBackground(player1Assigned, player2Assigned);  // Add color background according to its position, mainly use for Arduino
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

        // Display countdown timer
        if (totalTime <= 60)
        {
            player1CountDown.text = totalTime.ToString("F0");
            player2CountDown.text = totalTime.ToString("F0");
        }
    }

    public void P1KickIngredient(int index)
    {
        if (index >= 0 && index < player1Assigned.Count)
        {
            float originalPositionX = player1Assigned[index].transform.localPosition.x;
            float originalPositionY = player1Assigned[index].transform.localPosition.y;
            player2Assigned.Add(player1Assigned[index]);
            player1Assigned.RemoveAt(index);
        
            if (player2Assigned.Count == 0)
            {
                player2Assigned[0].transform.localPosition = new Vector3(-originalPositionX, originalPositionY, -1);
            }
            else
            {
                player2Assigned[player2Assigned.Count() - 1].transform.localPosition = new Vector3(-originalPositionX, originalPositionY, -1);
            }
        
            SortByYAxis(player1Assigned);
            SortByYAxis(player2Assigned);
        }
        
        AddBackground(player1Assigned, player2Assigned);
    }

    public void P2KickIngredient(int index)
    {
        if (index >= 0 && index < player2Assigned.Count)
        {
            float originalPositionX = player2Assigned[index].transform.localPosition.x;
            float originalPositionY = player2Assigned[index].transform.localPosition.y;
            player1Assigned.Add(player2Assigned[index]);
            player2Assigned.RemoveAt(index);
            if (player1Assigned.Count == 0)
            {
                player1Assigned[0].transform.localPosition = new Vector3(-originalPositionX, originalPositionY, -1);
            }
            else
            {
                player1Assigned[player1Assigned.Count() - 1].transform.localPosition = new Vector3(-originalPositionX, originalPositionY, -1);
            }
        
            SortByYAxis(player1Assigned);
            SortByYAxis(player2Assigned);
        }
        AddBackground(player1Assigned, player2Assigned);
    }

    // Add background on ingredients to indicate sensor
    public void AddBackground(List<GameObject> player1Assigned, List<GameObject> player2Assigned)
    {
        
        foreach (Transform child in GameObject.Find("PlayersIngredientBg").transform)
        {
            Destroy(child.gameObject);
        }
        
        for (int i = 0; i < player1Assigned.Count(); i++)
        {
            GameObject ingredientBg = new GameObject();
            ingredientBg.transform.SetParent(GameObject.Find("PlayersIngredientBg").transform);

            if (i == 0)
            {
                ingredientBg.name = "pinkBg";
            } else if (i == 1)
            {
                ingredientBg.name = "whiteBg";
            } else if (i == 2)
            {
                ingredientBg.name = "greenBg";
            } else if (i == 3)
            {
                ingredientBg.name = "beigeBg";
            }

            SpriteRenderer(ingredientBg);
            ingredientBg.transform.localScale = new Vector3(0.1f, 0.1f, -1f);
            ingredientBg.transform.localPosition = new Vector3(player1Assigned[i].transform.localPosition.x, player1Assigned[i].transform.localPosition.y, 0.5f);
        }
        
        for (int i = 0; i < player2Assigned.Count(); i++)
        {
            GameObject ingredientBg = new GameObject();
            ingredientBg.transform.SetParent(GameObject.Find("PlayersIngredientBg").transform);

            if (i == 0)
            {
                ingredientBg.name = "pinkBg";
            } else if (i == 1)
            {
                ingredientBg.name = "whiteBg";
            } else if (i == 2)
            {
                ingredientBg.name = "greenBg";
            } else if (i == 3)
            {
                ingredientBg.name = "beigeBg";
            }

            SpriteRenderer(ingredientBg);
            ingredientBg.transform.localScale = new Vector3(0.1f, 0.1f, -1f);
            ingredientBg.transform.position = new Vector3(player2Assigned[i].transform.position.x, player2Assigned[i].transform.position.y, player2Assigned[i].transform.position.z+0.2f);
        }
    }

    // Refresh the new round after yummy yummy
    public void InitialRound()
    {
        player1Assigned.Clear();
        player2Assigned.Clear();
        Array.Clear(player1Task, 0, player1Task.Length);
        Array.Clear(player2Task, 0, player1Task.Length);
    }

    // Reorder the ingredients according to its position
    private void SortByYAxis(List<GameObject> assignedList)
    {
        assignedList.Sort((a, b) => b.transform.position.y.CompareTo(a.transform.position.y));  // change the order of the ingredients based on position
    }

    // Check if the ingredients match recipe
    public bool CheckComplete()
    {
        if (player1Assigned.Count != 2 || player2Assigned.Count != 2)
        {
            return false;
        }

        // Check player 1's assigned items against their tasks
        foreach (var task in player1Task)
        {
            if (!player1Assigned.Any(ingredient => ingredient.name == task))
            {
                return false;
            }
        }

        // Check player 2's assigned items against their tasks
        foreach (var task in player2Task)
        {
            if (!player2Assigned.Any(ingredient => ingredient.name == task))
            {
                return false;
            }
        }

        return true; // All tasks matched
    }

    // For refresh a new round, delete all Game Object
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
        isPaused = true;
        StartCoroutine(PauseCoroutine());
    }

    // Waiting for 2 secs after yummy yummy
    private IEnumerator PauseCoroutine()
    {
        Time.timeScale = 0;  // Pause the game
        yield return new WaitForSecondsRealtime(2);  // Wait for 1 real seconds
        Time.timeScale = 1;  // Resume the game
        DestoryGameObject();
        InitialRound();
        TaskAssign();
        IngredientAssign();

        isPaused = false;  // Reset the pause flag
    }
    
    public void StartLight(string[] lightNames)
    {
        StartCoroutine(StartLightCoroutine(lightNames));
    }

    // Display countdown traffic light screen at the beginning of each game round
    private IEnumerator StartLightCoroutine(string[] lightNames)
    {
        for (int i = 0; i < lightNames.Length; i++)
        {
            GameObject player1Light = GameObject.Find("Player1Light");
            GameObject player2Light = GameObject.Find("Player2Light");
            GameObject p1TrafficLight = new GameObject(lightNames[i]);
            GameObject p2TrafficLight = new GameObject(lightNames[i]);
            p1TrafficLight.transform.SetParent(player1Light.transform);
            p2TrafficLight.transform.SetParent(player2Light.transform);
            p1TrafficLight.name = lightNames[i];
            p2TrafficLight.name = lightNames[i];
            SpriteRenderer(p1TrafficLight);
            SpriteRenderer(p2TrafficLight);
            p1TrafficLight.transform.localPosition = new Vector3(-4.5f, -0.2f, 3f);
            p1TrafficLight.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            p1TrafficLight.transform.rotation = Quaternion.Euler(0f, 0f, -90f);
            p2TrafficLight.transform.localPosition = new Vector3(5f, 0.1f, 3f);
            p2TrafficLight.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            p2TrafficLight.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
            
            // Wait for 1 second before proceeding to the next iteration
            yield return new WaitForSeconds(0.8f);
            
            foreach (Transform child in GameObject.Find("Player1Light").transform)
            {
                Destroy(child.gameObject);
            }
            foreach (Transform child in GameObject.Find("Player2Light").transform)
            {
                Destroy(child.gameObject);
            }
        }
    }

    // Display Times up look up screen
    public void TimesUp()
    {
        GameObject player1Light = GameObject.Find("Player1Light");
        GameObject player2Light = GameObject.Find("Player2Light");
        GameObject p1TimesUp = new GameObject();
        GameObject p2TimesUp = new GameObject();
        p1TimesUp.transform.SetParent(player1Light.transform);
        p2TimesUp.transform.SetParent(player2Light.transform);
        p1TimesUp.name = "timesUp";
        p2TimesUp.name = "timesUp";
        SpriteRenderer(p1TimesUp);
        SpriteRenderer(p2TimesUp);
        p1TimesUp.transform.position = new Vector3(-4.5f, 0.05f, 3f);
        p1TimesUp.transform.localScale = new Vector3(0.45f, 0.45f, 0.45f);
        p1TimesUp.transform.rotation = Quaternion.Euler(0f, 0f, -90f);
        p2TimesUp.transform.position = new Vector3(5f, 0.05f, 3f);
        p2TimesUp.transform.localScale = new Vector3(0.45f, 0.45f, 0.45f);
        p2TimesUp.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
    }

    private void OnApplicationQuit()
    {
        if (serialPort1 != null && serialPort1.IsOpen)
        {
            serialPort1.Close();
        }
        if (serialPort2 != null && serialPort2.IsOpen)
        {
            serialPort2.Close();
        }
    }
}