using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverScreen;
    public FadeToColor fadeToColor; // Referência ao script de fade
    public GameOverCamera gameOverCamera;

    void Start()
    {
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(false); // Garantindo que comece desativado
        }
    }

    public void AtivarGameOverScreen()
    {
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true); // Ativa o canvas de Game Over
            fadeToColor.StartFade();
            gameOverCamera.TriggerGameOver();
        }
        else
        {
            Debug.LogError("Game Over Screen não está associado!");
        }
    }
}

