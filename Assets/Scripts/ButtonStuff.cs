using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonStuff : MonoBehaviour
{
    public string sceneName;
    // Start is called before the first frame update
    public void loadScene()
    {
        Destroy(GameObject.Find("Network Manager"));
        SceneManager.LoadScene(sceneName);
       
    }
    public void exitGame()
    {
        Application.Quit();
    }
}
