using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class RespostaTrigger : MonoBehaviour
{
    public bool respostaCorreta;
    public GameOverManager gameOverManager;

    private void Start()
    {
        Debug.Log("RespostaTrigger inicializado. respostaCorreta: " + respostaCorreta + ", Ponto: " +gameObject.name);
        // Encontrar o GameOverManager na cena
    }

    private void OnTriggerEnter(Collider other)
    {
        gameOverManager = FindObjectOfType<GameOverManager>();
        if (gameOverManager == null)
        {
            Debug.LogError("GameOverManager ainda está nulo dentro do OnTriggerEnter!");
            return;  // Sai da função se não encontrar o GameOverManager
        }

        Debug.Log($"Colidindo com {other.gameObject.name} com tag {other.CompareTag("Player")}");
        
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entrou em contato com a resposta: " + gameObject.name + ", resposta: " + respostaCorreta);
            Player player = other.GetComponent<Player>();  // Obter o script do Player

            if (respostaCorreta)
            {
                // Jogador escolheu corretamente
                Debug.Log("Correto! Continue.");
            }
            else
            {
                // Jogador escolheu errado
                
                Debug.Log("Incorreto reposta! Game Over.");
                player.Die(0.5f);  // Aciona a animação de morte

                if (gameOverManager == null)
                {
                    Debug.LogError("GameOverManager está nulo dentro de OnTriggerEnter!");
                }
                else
                {
                    Debug.Log("Ativando tela de Game Over...");
                    Invoke("AtivarGameOver", 3f);
                }
                
                //Invoke("LoadGameOver", 3f);
                //Invoke("ShowGameOverScreen", 1f);  // Mostra a tela de Game Over após 1 segundo
            }
        }
    }

    private void AtivarGameOver()
    {
        gameOverManager.AtivarGameOverScreen();  // Ativa a tela de Game Over após 1 segundo
    }
}

