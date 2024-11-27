using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager singleton;

    public CarController carController;

    public TextMeshProUGUI speedText, timeText, scoreText;

    public float score = 0;

    public bool isGameOver = false;
    public bool isMoving = false;

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
        StartCoroutine(IncreaseCoin());
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Speed is " + carController.carSpeed.ToString("F2"));
        speedText.text = "Speed : " + carController.currentSpeed.ToString("F2") + "km/h";
        scoreText.text = "Score: " + score.ToString();
    }

    public void ApplyViolationFee(float fee)
    {
        score -= fee;
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
}
