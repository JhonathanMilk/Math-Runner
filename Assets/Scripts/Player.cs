using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController controller;
    private Animator animator;

    public float speed;
    public float jumpHeight;
    private float jumpVelocity;
    public float gravity;

    private bool isMovingLeft;
    private bool isMovingRight;

    private bool isDead;  // Estado de morte
    private bool isJumping;  // Estado de pulo

    public float horizontalSpeed;
    private float targetXPosition;
    private float[] positions = new float[] { -6f, 0f, 6f }; // Posições: esquerda, centro, direita
    private int currentPositionIndex = 1; // Inicia no centro

    public bool isGameOver = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        targetXPosition = transform.position.x;

        animator = GetComponentInChildren<Animator>();  // Referência ao componente Animator nas camadas abaixo (filhos)

        // Definindo que a animação inicial será a corrida
        isDead = false;
        isJumping = false;
    }

    void Update()
    {
        // Definindo a direção (eixo Z) sem usar deltaTime aqui para manter a velocidade consistente
        Vector3 direction = Vector3.forward * speed;

        if (isGameOver)
            return; // Ignora toda entrada se estiver em Game Over

        if (controller.isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
            {
                jumpVelocity = Mathf.Sqrt(2 * jumpHeight * gravity); // Cálculo da velocidade inicial do pulo
                animator.SetTrigger("JumpTrigger");  // Ativar o gatilho para pular
                isJumping = true;
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow) && !isMovingLeft && currentPositionIndex > 0)
            {
                StartCoroutine(MoveToPosition(positions[currentPositionIndex - 1]));
                currentPositionIndex--;
            }
            if (Input.GetKeyDown(KeyCode.RightArrow) && !isMovingRight && currentPositionIndex < positions.Length - 1)
            {
                StartCoroutine(MoveToPosition(positions[currentPositionIndex + 1]));
                currentPositionIndex++;
            }
        }
        else
        {
            jumpVelocity -= gravity * Time.deltaTime; // Aplica gravidade com base no tempo
        }

        // Finalizar pulo quando aterrissar
        if (controller.isGrounded && isJumping)
        {
            isJumping = false;
            animator.SetBool("isJumping", false);  // Voltar para a animação de corrida
        }

        // Aplicar a velocidade de pulo no eixo Y
        direction.y = jumpVelocity;

        // Aqui se aplica o deltaTime para garantir que o movimento seja suave e independente do FPS
        controller.Move(direction * Time.deltaTime);
    }

    IEnumerator MoveToPosition(float targetX)
    {
        float startX = transform.position.x;
        float elapsedTime = 0f;
        float moveDuration = 0.2f; // Tempo de movimentação

        isMovingLeft = targetX < transform.position.x;
        isMovingRight = targetX > transform.position.x;

        while (elapsedTime < moveDuration)
        {
            float newX = Mathf.Lerp(startX, targetX, elapsedTime / moveDuration);
            transform.position = new Vector3(newX, transform.position.y, transform.position.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = new Vector3(targetX, transform.position.y, transform.position.z);

        isMovingLeft = false;
        isMovingRight = false;
    }

    // Função para acionar o game over (morte)
    public void Die(float waitTime)
    {
        if (!isDead)
        {
            isDead = true;

            StartCoroutine(WaitAndPlayDeathAnimation(waitTime));  // Espera "waitTime" tempo antes de iniciar a animação de morte
        }
    }

    private IEnumerator WaitAndPlayDeathAnimation(float waitTime)
    {
        // Continua a movimentação normal por 1 segundo
        yield return new WaitForSeconds(waitTime);

        //parar a movimentação após a animação começar:
        controller.enabled = false;  // Desativa a movimentação do personagem após a morte, se necessário
        isGameOver = true;  //Desativar comandos

        // Após a espera, iniciar a animação de morte
        animator.SetBool("isDead", true);
    }

    // Função para colisão com obstáculo
    /*void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Obstacle"))
        {
            Die();  // Chamar a função de morte se colidir com um obstáculo
        }
    }*/
}



/*
speed = 20
jump height = 30
gravity = 1
horizontalspeed = 70


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController controller;
    private Animator animator;

    public float speed;
    public float jumpHeight;
    private float jumpVelocity;
    public float gravity;

    private bool isMovingLeft;
    private bool isMovingRight;

    private bool isDead;  // Estado de morte
    private bool isJumping;  // Estado de pulo

    public float horizontalSpeed;
    private float targetXPosition;
    private float[] positions = new float[] { -6f, 0f, 6f }; // Posições: esquerda, centro, direita
    private int currentPositionIndex = 1; // Inicia no centro

    public bool isGameOver = false;

    void Start()
    {
        
        controller = GetComponent<CharacterController>();
        targetXPosition = transform.position.x;

        //animator = GetComponent<Animator>();  // Referência ao componente Animator
        animator = GetComponentInChildren<Animator>();  //Referência ao componente Animator nas camadas abaixo (filhos)

        // Definindo que a animação inicial será a corrida
        isDead = false;
        isJumping = false;
    }
    
    void Update()
    {
        Vector3 direction = Vector3.forward * speed;

        if (isGameOver)
            return; // Ignora toda entrada se estiver em Game Over

        if (controller.isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
            {
                jumpVelocity = jumpHeight;
                animator.SetTrigger("JumpTrigger");  // Ativar o gatilho para pular
                isJumping = true;
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow) && !isMovingLeft && currentPositionIndex > 0)
            {
                StartCoroutine(MoveToPosition(positions[currentPositionIndex - 1]));
                currentPositionIndex--;
            }
            if (Input.GetKeyDown(KeyCode.RightArrow) && !isMovingRight && currentPositionIndex < positions.Length - 1)
            {
                StartCoroutine(MoveToPosition(positions[currentPositionIndex + 1]));
                currentPositionIndex++;
            }
        }
        else
        {
            jumpVelocity -= gravity;
        }

        // Finalizar pulo quando aterrissar
        if (controller.isGrounded && isJumping)
        {
            isJumping = false;
            animator.SetBool("isJumping", false);  // Voltar para a animação de corrida
        }

        direction.y = jumpVelocity;
        controller.Move(direction * Time.deltaTime);
    }

    IEnumerator MoveToPosition(float targetX)
    {
        float startX = transform.position.x;
        float elapsedTime = 0f;
        float moveDuration = 0.2f; // Tempo de movimentação

        isMovingLeft = targetX < transform.position.x;
        isMovingRight = targetX > transform.position.x;

        while (elapsedTime < moveDuration)
        {
            float newX = Mathf.Lerp(startX, targetX, elapsedTime / moveDuration);
            transform.position = new Vector3(newX, transform.position.y, transform.position.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = new Vector3(targetX, transform.position.y, transform.position.z);

        isMovingLeft = false;
        isMovingRight = false;
    }

    // Função para acionar o game over (morte)
    public void Die()
    {
        if (!isDead)
        {
            isDead = true;

            StartCoroutine(WaitAndPlayDeathAnimation(0.5f));  // Espera 1 segundo antes de iniciar a animação de morte
        }
    }

    private IEnumerator WaitAndPlayDeathAnimation(float waitTime)
    {
        // Continua a movimentação normal por 1 segundo
        yield return new WaitForSeconds(waitTime);

        // Opcionalmente, se quiser parar a movimentação após a animação começar:
        controller.enabled = false;  // Desativa o controle do personagem após a morte, se necessário
        isGameOver = true;  //Desativar comandos

        // Após a espera, iniciar a animação de morte
        animator.SetBool("isDead", true);

        
    }
}*/
