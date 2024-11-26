using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleTrigger : MonoBehaviour
{
    private GameOverManager gameOverManager;

    private void Start()
    {
        gameOverManager = FindObjectOfType<GameOverManager>();
        if (gameOverManager == null)
        {
            Debug.LogError("GameOverManager não encontrado!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))  // Verifica se colidiu com o Player
        {
            Debug.Log("Player colidiu com obstáculo: " + gameObject.name);

            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.Die(0f);  // Aciona a animação de morte do jogador, sem espera, imediatamente (0f)

                if (gameOverManager != null)
                {
                    Invoke("AtivarGameOver", 3f);
                }
                else
                {
                    Debug.LogError("GameOverManager não encontrado!");
                }
            }
        }
    }

    private void AtivarGameOver()
    {
        if (gameOverManager != null)
        {
            gameOverManager.AtivarGameOverScreen();
        }
    }
}
