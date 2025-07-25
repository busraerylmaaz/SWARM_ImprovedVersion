using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using System.Collections; // Coroutine i�in gerekli

public class SplashVideoController : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Inspector'dan atand���ndan emin olun
    public string nextSceneName = "Main Menu"; // Ge�ilecek sahne ad�

    void Awake()
    {
        // E�er Inspector'dan atanmad�ysa, otomatik bulmaya �al��
        if (videoPlayer == null)
        {
            videoPlayer = GetComponent<VideoPlayer>();
        }

        if (videoPlayer == null)
        {
            Debug.LogError("VideoPlayer component is missing on this GameObject or not assigned!");
            // VideoPlayer yoksa hemen sahneyi y�kle ve m�zi�i ba�lat (fallback)
            SceneManager.LoadScene(nextSceneName);
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayMusic();
            }
            return;
        }

        // Video bitti�inde �a�r�lacak olay� dinle
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        // Video bitti�inde DelayedMusicAndSceneLoad coroutine'ini ba�lat
        // Bu, m�zi�i ve sahne y�klemesini 0.5 saniye gecikmeyle yapacak.
        StartCoroutine(DelayedMusicAndSceneLoad(0.1f)); // 0.5 saniye gecikme
    }

    // M�zi�i ve sahne y�klemeyi gecikmeli yapan Coroutine
    private IEnumerator DelayedMusicAndSceneLoad(float delay)
    {
        // Belirtilen s�re kadar bekle
        yield return new WaitForSeconds(delay);

        // M�zik varsa ba�lat
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMusic();
            Debug.Log("Oyun m�zi�i videodan sonra " + delay + " saniye gecikmeyle ba�lad�.");
        }
        else
        {
            Debug.LogError("AudioManager.Instance mevcut de�il! M�zik ba�lat�lamad�.");
        }

        // Sonraki sahneyi y�kle
        SceneManager.LoadScene(nextSceneName);
    }

    void OnDestroy()
    {
        // Script yok edildi�inde olay� dinlemeyi b�rak
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnVideoEnd;
        }
        // Sahne de�i�ti�inde veya GameObject yok edildi�inde �al��an coroutine'leri durdur
        StopAllCoroutines();
    }
}