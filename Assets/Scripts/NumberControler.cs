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
        GerarNumerosNosPontos();
    }

    void Update()
    {
        if (jogador.position.z >= ultimaPosicaoGerada + distanciaParaGerarNumero)
        {
            GerarNumerosNosPontos();
            ultimaPosicaoGerada = jogador.position.z;
        }
    }

    public void AtualizarNumerosEOperacoes()
    {
        LimparNumerosEOperacoes();
        AtualizarPontosDeGeracao();
        GerarNumerosNosPontos(); // Atualiza com novos números e operações
    }

    void LimparNumerosEOperacoes()
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
        // Gerar a operação primeiro
        GerarOperacao();

        // Para exibir o primeiro número
        if (pontosDeGeracao.Count >= 3)
        {
            InstanciarNumeroOuOperacao(pontosDeGeracao[0], primeiroNumero.ToString());
            InstanciarNumeroOuOperacao(pontosDeGeracao[1], operacao); // Exibir a operação
            InstanciarNumeroOuOperacao(pontosDeGeracao[2], segundoNumero.ToString());

            // Gerar opções de respostas (esquerda e direita)
            if (pontosDeGeracao.Count >= 5)
            {
                // Randomizar a posição da resposta correta
                bool respostaCorretaNaEsquerda = Random.Range(0, 2) == 0;

                if (respostaCorretaNaEsquerda)
                {
                    // Resposta correta na direita, incorreta na esquerda
                    InstanciarNumeroOuOperacao(pontosDeGeracao[3], resultadoCorreto.ToString(), true);
                    InstanciarNumeroOuOperacao(pontosDeGeracao[4], resultadoIncorreto.ToString(), false);
                }
                else
                {
                    // Resposta correta na esquerda, incorreta na direita
                    InstanciarNumeroOuOperacao(pontosDeGeracao[3], resultadoIncorreto.ToString(), false);
                    InstanciarNumeroOuOperacao(pontosDeGeracao[4], resultadoCorreto.ToString(), true);
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
