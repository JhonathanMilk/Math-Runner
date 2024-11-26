using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NumberControler : MonoBehaviour
{
    public GameObject numeroPrefab; // Prefab do TextMeshPro
    public List<Transform> pontosDeGeracao = new List<Transform>(); // Lista de pontos de geração
    public float distanciaParaGerarNumero = 50f; // Distância necessária para gerar um número
    private Transform jogador;
    private float ultimaPosicaoGerada = 0f;

    private int primeiroNumero;
    private int segundoNumero;
    private string operacao;
    private int resultadoCorreto;
    private int resultadoIncorreto;

    void Start()
    {
        jogador = GameObject.FindGameObjectWithTag("Player").transform;
        AtualizarPontosDeGeracao();
    }

    void Update()
    {
        if (jogador.position.z >= ultimaPosicaoGerada + distanciaParaGerarNumero)
        {
            GerarNumerosNosPontos();
            ultimaPosicaoGerada = jogador.position.z;
        }
    }

    public void LimparNumerosEOperacoes(GameObject platform)
    {
        foreach (Transform ponto in pontosDeGeracao)
        {
            foreach (Transform child in ponto)
            {
                Destroy(child.gameObject);
            }
        }
    }

    // Função para instanciar números nos pontos de geração
    public void GerarNumerosNosPontos()
    {
        int numeroDePontos = pontosDeGeracao.Count;
        
        for (int i = 0; i < numeroDePontos; i += 5) // Processa a cada 5 pontos
        {
            // Gerar a operação uma vez para o conjunto de 5 pontos
            GerarOperacao();

            // Verificar se há pelo menos 5 pontos disponíveis
            if (i + 4 < numeroDePontos)
            {
                // Exibir o primeiro número no primeiro ponto do conjunto
                InstanciarNumeroOuOperacao(pontosDeGeracao[i], primeiroNumero.ToString());

                // Exibir a operação no segundo ponto
                InstanciarNumeroOuOperacao(pontosDeGeracao[i + 1], operacao);

                // Exibir o segundo número no terceiro ponto
                InstanciarNumeroOuOperacao(pontosDeGeracao[i + 2], segundoNumero.ToString());

                // Gerar opções de respostas (correta e incorreta)
                bool respostaCorretaNaEsquerda = Random.Range(0, 2) == 0;

                if (respostaCorretaNaEsquerda)
                {
                    // Resposta correta na esquerda, incorreta na direita
                    InstanciarNumeroOuOperacao(pontosDeGeracao[i + 3], resultadoCorreto.ToString(), true);
                    InstanciarNumeroOuOperacao(pontosDeGeracao[i + 4], resultadoIncorreto.ToString(), false);
                }
                else
                {
                    // Resposta incorreta na esquerda, correta na direita
                    InstanciarNumeroOuOperacao(pontosDeGeracao[i + 3], resultadoIncorreto.ToString(), false);
                    InstanciarNumeroOuOperacao(pontosDeGeracao[i + 4], resultadoCorreto.ToString(), true);
                }
            }
        }
    }


    GameObject InstanciarNumeroOuOperacao(Transform ponto, string valor, bool respostaCorreta = false)
    {
        if (ponto.childCount == 0)
        {
            // Instanciar o objeto de número ou operação
            GameObject numeroObj = Instantiate(numeroPrefab, ponto.position, Quaternion.identity);
            TextMeshPro textMeshPro = numeroObj.GetComponentInChildren<TextMeshPro>();
            textMeshPro.text = valor;
            numeroObj.transform.SetParent(ponto);

            // Se o ponto é um resultPoint, adicione a lógica para associar o trigger corretamente
            if (ponto.CompareTag("PontoDeGeracao") && ponto.GetComponent<RespostaTrigger>() != null)
            {
                // Atualizar o script existente ou realizar verificações, se necessário
                RespostaTrigger trigger = ponto.GetComponent<RespostaTrigger>();
                if (trigger != null)
                {
                    trigger.respostaCorreta = respostaCorreta;
                    Debug.Log("Atualizado ResultPoint com resposta correta: " + respostaCorreta);
                }
                else
                {
                    Debug.LogWarning("Não foi possível encontrar RespostaTrigger no ponto de resultado.");
                }
            }
            /*else
            {
                // Adiciona o script de verificação da resposta correta ao novo objeto instanciado
                RespostaTrigger trigger = numeroObj.AddComponent<RespostaTrigger>();
                trigger.respostaCorreta = respostaCorreta;
                Debug.Log("Número/Operação: " + valor + ", Resposta Correta: " + respostaCorreta);
            }*/

            return numeroObj;
        }
        return null;
    }


    public void AtualizarPontosDeGeracao()
    {
        pontosDeGeracao.Clear(); // Limpa a lista antes de buscar novos pontos
        GameObject[] pontos = GameObject.FindGameObjectsWithTag("PontoDeGeracao");
        
        foreach (GameObject ponto in pontos)
        {
            pontosDeGeracao.Add(ponto.transform);
        }

        Debug.Log(pontosDeGeracao.Count + " pontos de geração atualizados.");
    }

    void GerarOperacao()
    {
        primeiroNumero = Random.Range(1, 10);
        segundoNumero = Random.Range(1, 10);
        string[] operacoes = { "+", "-", "*", "/" };
        operacao = operacoes[Random.Range(0, operacoes.Length)];

        switch (operacao)
        {
            case "+":
                resultadoCorreto = primeiroNumero + segundoNumero;
                break;
            case "-":
                resultadoCorreto = primeiroNumero - segundoNumero;
                break;
            case "*":
                resultadoCorreto = primeiroNumero * segundoNumero;
                break;
            case "/":
                if (segundoNumero != 0)
                {
                    resultadoCorreto = primeiroNumero / segundoNumero;
                }
                break;
        }
        // Gera uma resposta incorreta para exibir
        resultadoIncorreto = resultadoCorreto + Random.Range(-3, 3); // Evitar resultado igual
        if (resultadoIncorreto == resultadoCorreto)
        {
            resultadoIncorreto += 1;
        }
    }
}
