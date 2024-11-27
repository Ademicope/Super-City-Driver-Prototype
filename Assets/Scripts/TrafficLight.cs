using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLight : MonoBehaviour
{
    public List<GameObject> lights = new List<GameObject>();
    public GameManager gameManager;

    public float lightDuration = 3f; // Duration for each light
    private int currentLightIndex = 0; // Tracks the active light

    // Start is called before the first frame update
    void Start()
    {
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }
        if (lights.Count > 0)
        {
            StartCoroutine(SwitchTrafficLight());
            Debug.Log("switching lights");
        }
        else
        {
            Debug.LogError("No traffic lights assigned!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //LightSwitch();
    }

    IEnumerator SwitchTrafficLight()
    {
        while (!gameManager.isGameOver)
        {
            // Activate current light and deactivate all others
            for (int i = 0; i < lights.Count; i++)
            {
                lights[i].SetActive(i == currentLightIndex);
            }

            // Wait for the duration before switching to the next light
            yield return new WaitForSeconds(lightDuration);

            // Move to the next light in the list
            currentLightIndex = (currentLightIndex + 1) % lights.Count;
        }
    }
}
