using System.Collections;
using UnityEngine;

public class GameOverCamera : MonoBehaviour
{
    public Transform targetPosition;  // A posição final para onde a câmera vai se mover
    public float transitionDuration = 5.0f;  // Tempo de transição
    public Vector3 finalRotation = new Vector3(90f, 0f, 0f);  // Rotação final da câmera (vista de cima)

    private void Start()
    {
        // Nenhuma necessidade de armazenar a posição inicial aqui
    }

    public void TriggerGameOver()
    {
        // Iniciar a transição da câmera
        StartCoroutine(TransitionToAerialView());
    }

    private IEnumerator TransitionToAerialView()
    {
        float elapsedTime = 0;

        // Usar a posição e rotação atuais da câmera no momento em que o game over acontece
        Vector3 currentPosition = transform.position;
        Quaternion currentRotation = transform.rotation;

        while (elapsedTime < transitionDuration)
        {
            // Interpolando a posição da câmera entre o ponto atual e o ponto final
            transform.position = Vector3.Lerp(currentPosition, targetPosition.position, elapsedTime / transitionDuration);

            // Interpolando a rotação da câmera para a rotação final (vista de cima)
            transform.rotation = Quaternion.Lerp(currentRotation, Quaternion.Euler(finalRotation), elapsedTime / transitionDuration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Garante que a câmera chegue exatamente na posição final e rotação final após o tempo de transição
        transform.position = targetPosition.position;
        transform.rotation = Quaternion.Euler(finalRotation);
    }
}
