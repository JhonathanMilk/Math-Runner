using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField]private string levelName;
    //[SerializeField]private GameObject painelMainMenu;
    
    public void LoadScene(string cena)
    {
        SceneManager.LoadScene(cena);
    }

    public void StartButton ()
    {
        SceneManager.LoadScene(levelName);
    }

    public void QuitButton ()
    {
        Debug.Log("Game exit");
        Application.Quit();
    }
}
