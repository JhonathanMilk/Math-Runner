using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RespostaTrigger : MonoBehaviour
{
    public bool respostaCorreta;

    private void Start()
    {
        Debug.Log("RespostaTrigger inicializado. respostaCorreta: " + respostaCorreta + ", Ponto: " +gameObject.name);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Colidindo com {other.gameObject.name} com tag {other.CompareTag("Player")}");
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entrou em contato com a resposta: " + gameObject.name + ", resposta: " + respostaCorreta);
            if (respostaCorreta)
            {
                // Jogador escolheu corretamente
                Debug.Log("Correto! Continue.");
            }
            else
            {
                // Jogador escolheu errado
                Debug.Log("Incorreto reposta! Game Over.");
                Invoke("LoadGameOver", 3f);
            }
        }
    }

    void LoadGameOver ()
    {
        SceneManager.LoadScene("GameOver");
    }
}

