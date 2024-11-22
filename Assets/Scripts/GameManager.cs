using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;

    public CarController carController;

    public TextMeshProUGUI speedText, timeText, coinText;

    private float time = 20;
    private float coin = 0;

    public bool isGameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        if (gameManager == null)
        {
            gameManager = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Speed is " + carController.carSpeed.ToString("F2"));

        speedText.text = "Speed : " + carController.currentSpeed.ToString("F2") + "km/h";

    }

    public void CheckRules()
    {

    }
}
