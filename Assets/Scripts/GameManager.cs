using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager singleton;

    public CarController carController;

    public GameObject startMenu;
    public GameObject gameOverPanel;
    public GameObject missionCompletePanel;
    public GameObject scorePanel;
    public Button restartButton;
    public Button startButton;

    public TextMeshProUGUI speedText, timeText, scoreText, gameOverText, violationText;
    public Rigidbody car;
    public float score = 0;

    public bool isGameOver = false;
    public bool isMoving = false;

    public float violationCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (singleton == null)
        {
            singleton = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        startMenu.SetActive(true);
        gameOverPanel.SetActive(false);
        scorePanel.SetActive(false);
        startButton.onClick.AddListener(StartGame);
        restartButton.onClick.AddListener(RestartGame);
        Time.timeScale = 0f;
        StartCoroutine(IncreaseCoin());
    }

    // Update is called once per frame
    void Update()
    {
        speedText.text = "Speed : " + carController.currentSpeed.ToString("F2") + "km/h";
        scoreText.text = "Score: " + score.ToString();
        violationText.text = "Violations: " + violationCount.ToString();
        if (violationCount == 4)
        {
            GameOver();
        }
    }

    public void ApplyViolationFee(float fee)
    {
        score -= fee;
        violationCount++;
        Debug.Log("Score balance: " + score);
    }
    IEnumerator IncreaseCoin()
    {
        
        while (!isGameOver)
        {
            scoreText.text = "Score: " + score;
            yield return new WaitForSeconds(1);
            if (carController.currentSpeed > 0)
            {
                isMoving = true;
                score++;
            }
            
        }
    }

    public void StartGame()
    {
        startMenu.SetActive(false);
        scorePanel.SetActive(true);
        Time.timeScale = 1.0f;
        isGameOver = false;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GameOver()
    {
        isGameOver = true;
        car.velocity = Vector3.zero;
        gameOverText.gameObject.SetActive(true);
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void GameWon()
    {
        isGameOver = true;
        car.velocity = Vector3.zero;
        //gameOverText.gameObject.SetActive(true);
        missionCompletePanel.SetActive(true);
        Time.timeScale = 0f;
    }
}
