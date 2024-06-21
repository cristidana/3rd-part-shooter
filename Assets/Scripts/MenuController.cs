using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] private AudioClip clickSound;

    private AudioSource audioSource;

    void Start()
    {

        audioSource = GetComponent<AudioSource>();
    }
    public void StartGame()
    {
        PlayClickSound();
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Level"); 
    }

   
    public void ExitGame()
    {
        PlayClickSound();
        Application.Quit();
    }

    private void PlayClickSound()
    {
        if (clickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }
}
