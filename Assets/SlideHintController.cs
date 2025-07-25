using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class SlideHintController : MonoBehaviour
{
    public TextMeshProUGUI slideHintText;
    public float fadeDuration = 1f;
    public float displayDuration = 4f;
    public float moveOffset = 30f; // ne kadar yukarý kayarak gelecek

    private RectTransform rectTransform;
    private Vector2 originalPosition;

    private void Start()
    {
        if (SceneManager.GetActiveScene().name.ToLower().Contains("level1"))
        {
            rectTransform = slideHintText.GetComponent<RectTransform>();
            originalPosition = rectTransform.anchoredPosition;

            string message = LocalizationManager.Instance.GetLocalizedValue("slide_hint");
            slideHintText.text = message;

            // yazýyý baþlangýçta aþaðýda ve görünmez yap
            rectTransform.anchoredPosition = originalPosition - new Vector2(0, moveOffset);
            SetAlpha(0f);

            slideHintText.gameObject.SetActive(true);
            StartCoroutine(AnimateHint());
        }
    }

    private IEnumerator AnimateHint()
    {
        // fade in + yukarý kay
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            float t = elapsed / fadeDuration;
            SetAlpha(Mathf.Lerp(0, 1, t));
            rectTransform.anchoredPosition = Vector2.Lerp(originalPosition - new Vector2(0, moveOffset), originalPosition, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        SetAlpha(1f);
        rectTransform.anchoredPosition = originalPosition;

        yield return new WaitForSeconds(displayDuration);

        // fade out
        elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            float t = elapsed / fadeDuration;
            SetAlpha(Mathf.Lerp(1, 0, t));
            elapsed += Time.deltaTime;
            yield return null;
        }
        SetAlpha(0f);
        slideHintText.gameObject.SetActive(false);
    }

    private void SetAlpha(float alpha)
    {
        Color c = slideHintText.color;
        slideHintText.color = new Color(c.r, c.g, c.b, alpha);
    }
}
