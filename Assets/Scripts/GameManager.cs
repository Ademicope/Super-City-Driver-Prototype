using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager singleton;

    public CarController carController;

    public TextMeshProUGUI speedText, timeText, coinText;

    public float coin = 0;

    public bool isGameOver = false;
    public bool isMoving = false;

    // Start is called before the first frame update
    void Start()
    {
        if (singleton == null)
        {
            singleton = this;
        }
        StartCoroutine(IncreaseCoin());
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Speed is " + carController.carSpeed.ToString("F2"));
        speedText.text = "Speed : " + carController.currentSpeed.ToString("F2") + "km/h";
        coinText.text = "Coins: " + coin.ToString();
    }

    public void ApplyViolationFee(float fee)
    {
        coin -= fee;
        Debug.Log("Coin balance: " + coin);
    }
    IEnumerator IncreaseCoin()
    {
        
        while (!isGameOver)
        {
            coinText.text = "Coin: " + coin;
            yield return new WaitForSeconds(1);
            if (carController.currentSpeed > 0)
            {
                isMoving = true;
                coin++;
            }
            
        }
    }
}
