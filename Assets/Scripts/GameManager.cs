using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject winPanel;
    private List<EnemyController> enemies;
    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemies = new List<EnemyController>(FindObjectsOfType<EnemyController>());
        winPanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ReturnToMainMenu();
        }
    }

    public void EnemyDestroyed(EnemyController enemy)
    {
        enemies.Remove(enemy);
        CheckWinCondition();
    }

    private void CheckWinCondition()
    {
        if (enemies.Count == 0)
        {
            WinGame();
        }
    }

    private void WinGame()
    {
        winPanel.SetActive(true);
        player.gameObject.GetComponent<ShooterController>().setPanelControl();
    }

    public void ReturnToMainMenu()
    {
        player.gameObject.GetComponent<ShooterController>().setPanelControl();
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MainMenu"); 
    }
}
