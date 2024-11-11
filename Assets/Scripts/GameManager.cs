using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;

    public CarController carController;

    public TextMeshProUGUI speedText;

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
        Debug.Log("Speed is " + carController.carSpeed.ToString("F2"));

        speedText.text = "Speed : " + carController.carSpeed.ToString("F2");

    }
}
