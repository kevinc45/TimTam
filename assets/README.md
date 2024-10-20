# Instruction of Prototype

## How to set up Chef Cross on your computer?

To deploy and use the Chef Cross project, follow these steps:

### 1. Download the project assets

- [Click here](https://github.com/kevinc45/TimTam/tree/main/assets/Assets) to download the Assets folder from our GitHub repository.

### 2. Create a New Unity Project

- Open Unity Hub and create a new 2D Unity project.
- Name the project as you like and set the location where you'd like to store it.

### 3. Add Assets to Your Project

- Once your Unity project is created, open the project folder.
- Drag and drop the Assets folder that you downloaded into your project’s root directory.
- If Unity asks to replace any existing files, confirm the replacement to ensure that the correct assets are loaded.
  <img width="229" alt="ProjectStructure" src="https://github.com/user-attachments/assets/b2fbcf81-27c4-4c62-bfde-5ff5b1eae95c">

### 4. Ensure the Arduino Port Number

- When you open the [code in assets](https://github.com/kevinc45/TimTam/blob/main/codes/Assets/Script/Main.cs), you want to check especially on this line:
```cs
    public string portName1 = "COM16";
    public string portName2 = "COM19"; 
```
- Fill those with the port number where your Arduino is connected.
  - **Windows:** Open `Device Manager` and find `Ports (COM & LPT)`. If connected properly, both Arduino COM numbers should appear there.
  - **Mac:** Open `Terminal` and enter `ls /dev/cu.*`. To identify Arduino ports, it will likely be something like `/dev/cu.usbmodemXXXX` or `/dev/cu.usbserialXXXX`.
  - Replace the value of both `portName1` and `portName2` in the code above with the value of your Arduino ports.


### 5. Resolve Game Object Reference Errors (Optional)

- If you encounter any errors in the Unity Console about Missing Game Object References, double-check that the **Recipe** object is properly referenced in the scene.
- To verify ensure that it is correctly referenced, as shown below:
  <img width="762" alt="UnityHierachy" src="https://github.com/user-attachments/assets/16590bef-e58d-40f6-bf1c-83d60f9ed68d">

### 6. Launch and Test the Prototype

- After setting up the assets, click Play in Unity to test the prototype.

## How to play Chef Cross

| How to Chef Cross                                                                                    |
| ---------------------------------------------------------------------------------------------------- |
| ![Game Instruction](https://github.com/user-attachments/assets/b02d802f-cf5a-46d5-b5a2-ef900b21caec) |
| **[A] Ingredient**: Kickable items passed between two players.                                       |
| **[B] Recipe**: Task requiring each player to collect specific ingredients.                          |
| **[C] Countdown Timer**: Game time synced with traffic light duration.                               |
| **[D] Indicator**: Sensor triggers kicks based on the background colour.                             |
| **[E] Start**: The game begins with a 3-second countdown after the traffic light turns red.          |
| **[F] Yummy**: Displays when ingredients collected match the recipe.                                 |
| **[G] Times Up**: The game stops few seconds before the traffic light turns green.                   |

| Gameplay                                                                                   | Step                                                                                                                                        |
| ------------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------------------------------------------- |
| ![step 1](https://github.com/user-attachments/assets/deb840e7-bf6e-47a8-9f77-e4b3f8571eaa) | 1. The game starts after a 3-second countdown when the light turns red.                                                                     |
| ![step 2](https://github.com/user-attachments/assets/9a1bd85c-0648-4413-a662-804348156b9c) | 2. Players can kick any ingredient to pass it to the other player. For the prototype, step on the corresponding color to simulate the kick. |
| ![step 3](https://github.com/user-attachments/assets/a1f2ae2c-2f8a-4f7f-bc51-31b4194acde2) | 3. When Player 1 kicks the banana, it passes to Player 2’s screen, and the background changes to green (position 3).                        |
| ![step 4](https://github.com/user-attachments/assets/e6990509-2126-4918-b80b-74bca6a990be) | 4. If both players' ingredients match the recipe, "Yummy Yummy" will appear, and the cycle repeats.                                         |
| ![step 5](https://github.com/user-attachments/assets/9b0bb5a5-c3b6-4e24-8502-5f02925511f9) | 5. When the light turns green, "Times Up! Look Up!" prompts players to cross safely.                                                        |

---

### Note for Game Logic:

1. **Randomised Ingredient Assignment**: All kickable ingredients are randomly assigned to positions on the opposite player’s side once the corresponding keys are pressed or sensors are triggered. The sequence follows a logical progression from top to bottom.
2. **Limited Ingredient Spawn**: At the start of the game, only 4 kickable ingredients will be spawned. Each player can have a minimum of 0 and a maximum of 4 kickable ingredients on their plates at any given time.
3. **Gameplay Timer**: The game is timed for 60 seconds. Players aim to score “Yummy” within this time limit by matching ingredients to their recipe.

### Play with Keyboard

1. **Player 1 (left player)**: Controls the ingredients using the **Q, W, E, R** keys, reading the “ingredients” from top to bottom.
2. **Player 2 (right player)**: Controls the ingredients using the **U, I, O, P** keys, also reading from top to bottom.

### Play with Arduino

1. Both players hover their feet over the corresponding sensor to trigger the kicking action.

---
