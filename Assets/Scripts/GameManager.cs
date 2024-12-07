using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //public static GameManager singleton;

    public CarController carController;

    public GameObject startMenu;
    public GameObject gameOverPanel;
    public GameObject missionCompletePanel;
    public GameObject scorePanel;
    public Button restartButton;
    public Button startButton;

    public TextMeshProUGUI speedText, timeText, scoreText, gameOverText, violationText;
    public GameObject violationPanel;
    public Rigidbody car;
    public float score = 0;

    public bool isGameOver = false;
    public bool isMoving = false;

    public float violationCount = 0;

    public AudioSource gameAudio;
    public AudioClip gameSound;
    public AudioClip moveSound, startSound;

    public ParticleSystem smoke;

    // Start is called before the first frame update
    void Start()
    {
        if (gameAudio == null)
        {
            gameAudio = GetComponent<AudioSource>();
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
        if (!isGameOver && !gameAudio.isPlaying)
        {
            gameAudio.clip = gameSound;
            gameAudio.Play();
        }
        speedText.text = "Speed: " + carController.currentSpeed.ToString("F2") + "km/h";
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
        StartCoroutine(ShowViolation());
        scoreText.text = "Score: " + score.ToString();
        violationText.text = "Violations: " + violationCount.ToString();

        Debug.Log("Score balance: " + score);
    }

    IEnumerator ShowViolation()
    {
        violationPanel.SetActive(true);
        yield return new WaitForSeconds(1);
        violationPanel.SetActive(false);
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

    private void SoundAndEffects()
    {
        if (isMoving)
        {
            gameAudio.clip = moveSound;
            gameAudio.Play();
            smoke.Play();
        }
    }

    public void StartGame()
    {
        startMenu.SetActive(false);
        scorePanel.SetActive(true);
        Time.timeScale = 1.0f;
        isGameOver = false;

        gameAudio.clip = gameSound;
        gameAudio.Play();
    }

    public void RestartGame()
    {
        //Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GameOver()
    {
        isGameOver = true;
        car.velocity = Vector3.zero;

        gameAudio.Stop();

        gameOverText.gameObject.SetActive(true);
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void GameWon()
    {
        isGameOver = true;
        car.velocity = Vector3.zero;
        missionCompletePanel.SetActive(true);
        Time.timeScale = 0f;
    }
}
