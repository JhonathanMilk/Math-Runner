using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObstacleSpawner : MonoBehaviour
{
    public List<GameObject> obstaculos; // Lista de tipos de obstáculos
    public float zMinOffset = 0f; // Mínima distância do jogador para o obstáculo aparecer
    public float zMaxOffset = 100f; // Máxima distância do jogador para o obstáculo aparecer
    private Transform jogador;
    private Transform[] plataformas;
    private float[] xPositions = { -6f, 0f, 6f }; // Posições laterais (esquerda, centro, direita)
    
    // Lista para manter os obstáculos instanciados
    private List<GameObject> obstaculosInstanciados = new List<GameObject>();

    void Start()
    {
        jogador = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void InitializeObstacleSpawning()
    {
        plataformas = GameObject.FindGameObjectsWithTag("Platform")
            .Select(platform => platform.transform)
            .ToArray();

        if (plataformas.Length > 0)
        {
            foreach (Transform plataforma in plataformas)
            {
                CriarObstaculosNaPlataforma(plataforma); // Criar obstáculos na primeira vez
            }
        }
        else
        {
            Debug.LogWarning("Nenhuma plataforma foi encontrada com a tag 'Platform'!");
        }
    }

    public void RepositionObstacles()
    {
        foreach (Transform plataforma in plataformas)
        {
            ReposicionarObstaculosNaPlataforma(plataforma); // Reposicionar obstáculos já criados
        }
    }

    // Função para criar os obstáculos na primeira vez
    public void CriarObstaculosNaPlataforma(Transform plataforma)
    {
        int numeroDeObstaculos = Random.Range(10, 20);

        for (int i = 0; i < numeroDeObstaculos; i++)
        {
            // Escolhe um obstáculo aleatoriamente da lista
            GameObject obstaculoEscolhido = obstaculos[Random.Range(0, obstaculos.Count)];

            // Define a posição e rotação aleatória
            Vector3 posicaoAleatoria = GerarPosicaoAleatoria(plataforma);
            Quaternion rotacaoOriginal = obstaculoEscolhido.transform.rotation;

            // Instancia o obstáculo e adiciona à lista de obstáculos instanciados
            GameObject obstaculoInstanciado = Instantiate(obstaculoEscolhido, posicaoAleatoria, rotacaoOriginal);
            obstaculosInstanciados.Add(obstaculoInstanciado);
        }
    }

    // Função para reposicionar os obstáculos já criados
    public void ReposicionarObstaculosNaPlataforma(Transform plataforma)
    {
        // Verifica se já temos obstáculos instanciados
        if (obstaculosInstanciados.Count > 0)
        {
            for (int i = 0; i < obstaculosInstanciados.Count; i++)
            {
                //reposicionar apenas os objetos da plataforma passada, ao qual agora está na frente 
                if(obstaculosInstanciados[i].transform.position.z+263 < plataforma.position.z)
                {
                    ReposicionarObstaculo(obstaculosInstanciados[i], plataforma);
                }
            }
        }
    }

    private Vector3 GerarPosicaoAleatoria(Transform plataforma)
    {
        // Define a posição aleatória na profundidade (Z)
        float zRandom = plataforma.position.z + Random.Range(zMinOffset, zMaxOffset);

        // Define a posição aleatória na lateralidade (X)
        float xRandom = xPositions[Random.Range(0, xPositions.Length)];

        return new Vector3(xRandom, plataforma.position.y + 0.5f, zRandom);
    }

    private void ReposicionarObstaculo(GameObject obstaculo, Transform plataforma)
    {
        Vector3 novaPosicao = GerarPosicaoAleatoria(plataforma);
        obstaculo.transform.position = novaPosicao;
    }
}

