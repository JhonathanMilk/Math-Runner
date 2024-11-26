using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlatform : MonoBehaviour
{
    public List<GameObject> platforms = new List<GameObject>();
    public List<Transform> currentPlatforms = new List<Transform>();
    public int offset;
    private Transform player;
    private Transform currentPlatformPoint;
    private int platformIndex;
    private NumberControler controladorDeNumeros;
    private int plataformasRecicladas = 0; // Contador de plataformas recicladas
    ObstacleSpawner obstacleSpawner;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        controladorDeNumeros = FindObjectOfType<NumberControler>(); // Certificar que está atribuindo o Controlador de Números

        for(int i = 0 ; i < platforms.Count; i++)
        {
            Transform p = Instantiate(platforms[i], new Vector3(0, 0, i * 263), transform.rotation).transform;
            currentPlatforms.Add(p);
            offset += 263;

            if (!p.gameObject.activeSelf)
            {
                Debug.LogWarning("Plataforma instanciada, mas está desativada! Ativando manualmente.");
                p.gameObject.SetActive(true);
            }
        }
        controladorDeNumeros.AtualizarPontosDeGeracao();
        currentPlatformPoint = currentPlatforms[platformIndex].transform.GetChild(2).GetComponent<Platform>().point;

        // Após instanciar e ativar as plataformas, avisa o ObstacleSpawner
        obstacleSpawner = FindObjectOfType<ObstacleSpawner>();
        if (obstacleSpawner != null)
        {
            obstacleSpawner.InitializeObstacleSpawning();
        }
    }

    void Update()
    {
        float distance = player.position.z - currentPlatformPoint.position.z;

        if (distance >= 8)
        {
            Recycle(currentPlatforms[platformIndex].gameObject);
            platformIndex++;

            if(platformIndex > currentPlatforms.Count - 1)
            {
                platformIndex = 0;
            }

            currentPlatformPoint = currentPlatforms[platformIndex].transform.GetChild(2).GetComponent<Platform>().point;
            
        }
    }

    public void Recycle(GameObject platform)
    {
        // Limpa somente os pontos da plataforma reciclada
        controladorDeNumeros.LimparNumerosEOperacoes(platform);

        platform.transform.position = new Vector3(0, 0, offset);

        offset += 263;

        // Atualiza os números e operações apenas nos pontos dessa plataforma reciclada
        controladorDeNumeros.GerarNumerosNosPontos();
        obstacleSpawner.ReposicionarObstaculosNaPlataforma(platform.transform);
    }
}
