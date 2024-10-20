using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject quit;
    public GameObject restart;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }
    }
    
    void OnMouseDown()
    {
        if (gameObject.name == "Restart")
        {
            Restart();
        }
        else if (gameObject.name == "Quit"){
            QuitGame();
        }
    }

    // Restart the game: press the restart btn
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    // Close the game: press the close icon and esc
    public void QuitGame()
    {
        Application.Quit();
        // UnityEditor.EditorApplication.isPlaying = false;  // Cannot run in build apk
    }
}
