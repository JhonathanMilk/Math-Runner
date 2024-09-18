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
        isDead = true;
        animator.SetBool("isDead", true);  // Ativar a animação de morte
    }

    // Função para colisão com obstáculo
  /*  void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Obstacle"))
        {
            Die();  // Chamar a função de morte se colidir com um obstáculo
        }
    }*/
}


/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController controller;

    public float speed;
    public float jumpHeight;
    private float jumpVelocity;
    public float gravity;

    private bool isMovingLeft;
    private bool isMovingRigt;

    public float horizontalSpeed;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = Vector3.forward * speed;

        if (controller.isGrounded)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                jumpVelocity = jumpHeight;
            }
            if(Input.GetKeyDown(KeyCode.LeftArrow) && transform.position.x > -3f && !isMovingLeft)
            {
                StartCoroutine(LeftMove());
            }
            if(Input.GetKeyDown(KeyCode.RightArrow) && transform.position.x < 3f && !isMovingRigt)
            {
                StartCoroutine(RightMove());
            }
        }
        else
        {
            jumpVelocity -= gravity;
        }

        direction.y = jumpVelocity;
        controller.Move(direction * Time.deltaTime);
    }

    IEnumerator LeftMove()
    {
        for(float i = 0; i<10; i+=0.1f)
        {
            if(transform.position.x < -3f)
            {
                break;
            }
            controller.Move(Vector3.left * Time.deltaTime *horizontalSpeed);
            yield return null;
        }
    }

    IEnumerator RightMove()
    {
        for(float i = 0; i<10; i+=0.1f)
        {
            controller.Move(Vector3.right * Time.deltaTime *horizontalSpeed);
            if(transform.position.x > 3f)
            {
                break;
            }
            yield return null;
        }
    }
}*/
