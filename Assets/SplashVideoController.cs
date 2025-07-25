using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using System.Collections; // Coroutine için gerekli

public class SplashVideoController : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Inspector'dan atandýðýndan emin olun
    public string nextSceneName = "Main Menu"; // Geçilecek sahne adý

    void Awake()
    {
        // Eðer Inspector'dan atanmadýysa, otomatik bulmaya çalýþ
        if (videoPlayer == null)
        {
            videoPlayer = GetComponent<VideoPlayer>();
        }

        if (videoPlayer == null)
        {
            Debug.LogError("VideoPlayer component is missing on this GameObject or not assigned!");
            // VideoPlayer yoksa hemen sahneyi yükle ve müziði baþlat (fallback)
            SceneManager.LoadScene(nextSceneName);
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayMusic();
            }
            return;
        }

        // Video bittiðinde çaðrýlacak olayý dinle
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        // Video bittiðinde DelayedMusicAndSceneLoad coroutine'ini baþlat
        // Bu, müziði ve sahne yüklemesini 0.5 saniye gecikmeyle yapacak.
        StartCoroutine(DelayedMusicAndSceneLoad(0.1f)); // 0.5 saniye gecikme
    }

    // Müziði ve sahne yüklemeyi gecikmeli yapan Coroutine
    private IEnumerator DelayedMusicAndSceneLoad(float delay)
    {
        // Belirtilen süre kadar bekle
        yield return new WaitForSeconds(delay);

        // Müzik varsa baþlat
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMusic();
            Debug.Log("Oyun müziði videodan sonra " + delay + " saniye gecikmeyle baþladý.");
        }
        else
        {
            Debug.LogError("AudioManager.Instance mevcut deðil! Müzik baþlatýlamadý.");
        }

        // Sonraki sahneyi yükle
        SceneManager.LoadScene(nextSceneName);
    }

    void OnDestroy()
    {
        // Script yok edildiðinde olayý dinlemeyi býrak
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnVideoEnd;
        }
        // Sahne deðiþtiðinde veya GameObject yok edildiðinde çalýþan coroutine'leri durdur
        StopAllCoroutines();
    }
}