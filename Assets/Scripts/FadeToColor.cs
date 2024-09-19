using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class FadeToColor : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 1f; // Duração do fade
    private Color targetColor = new Color(1, 0, 0, 0.5f); // Vermelho com 80% de opacidade

    public void StartFade()
    {
        StartCoroutine(FadeCoroutine());
    }

    private IEnumerator FadeCoroutine()
    {
        float elapsedTime = 0f;
        Color startingColor = fadeImage.color;

        while (elapsedTime < fadeDuration)
        {
            fadeImage.color = Color.Lerp(startingColor, targetColor, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Garantir que a cor final seja o alvo
        fadeImage.color = targetColor;
    }
}

