using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject quit;
    public GameObject restart;
    // Start is called before the first frame update
    void Start()
    {
        
    }

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
            Debug.Log("restart");
            Restart();
        }
        else if (gameObject.name == "Quit"){
            Debug.Log("quit");
            QuitGame();
        }
    }

    // restart the game: press the restart btn
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    // close the game: press the close icon and esc
    public void QuitGame()
    {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
