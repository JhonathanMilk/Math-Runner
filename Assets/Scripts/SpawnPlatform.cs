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

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        controladorDeNumeros = FindObjectOfType<NumberControler>(); // Certificar que está atribuindo o Controlador de Números

        for(int i = 0 ; i < platforms.Count; i++)
        {
            Transform p = Instantiate(platforms[i], new Vector3(0, 0, i * 86), transform.rotation).transform;
            currentPlatforms.Add(p);
            offset += 86;
        }
        controladorDeNumeros.AtualizarPontosDeGeracao();
        currentPlatformPoint = currentPlatforms[platformIndex].GetComponent<Platform>().point;
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

            currentPlatformPoint = currentPlatforms[platformIndex].GetComponent<Platform>().point;
        }
    }

    public void Recycle(GameObject platform)
    {
        //controladorDeNumeros.LimparNumerosAposPassar(player);

        platform.transform.position = new Vector3(0, 0, offset);
        //controladorDeNumeros.MoverPontosDeGeracao(platform.transform, offset); // Mover os pontos de geração junto com a plataforma
        offset += 86;

        plataformasRecicladas++;
        if (plataformasRecicladas >= currentPlatforms.Count)
        {
            controladorDeNumeros.AtualizarNumerosEOperacoes(); // Atualiza números e operações após reciclar todas as plataformas
            plataformasRecicladas = 0; // Reseta o contador de plataformas recicladas
        }
        //controladorDeNumeros.GerarNumerosNosPontos();
    }
}
