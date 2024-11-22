using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficRuleTrigger : MonoBehaviour
{
    public enum Rule { SpeedCheck, Red, Yellow, Green, PedestrianCrossing }

    public Rule rule;

    public float maxAllowedSpeed = 50f;
    public float stopDuration = 3f;

    public float violationFee = 0;

    CarController car;

    private void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            car = other.GetComponent<CarController>();
            if (rule == Rule.PedestrianCrossing)
            {
                violationFee += 5;
                Debug.Log("Speed violation!! lost 5 coins");
                NotifyGameManager(violationFee, "Speed violation!! lost 5 coins");
            }
            if (rule == Rule.SpeedCheck)
            {
                if (car != null && car.currentSpeed > maxAllowedSpeed)
                {
                    violationFee += 15;
                    Debug.Log("Speed violation!! lost 5 coins");
                }
            }
            if (rule == Rule.Red || rule == Rule.Yellow)
            {
                violationFee += 10;
                Debug.Log("Traffic light violation!! lost 10 coins");
                NotifyGameManager(violationFee, "Traffic light violation!! lost 10 coins");
            }
        }
    }

    private void NotifyGameManager(float fee, string message)
    {
        if (GameManager.singleton != null)
        {
            GameManager.singleton.ApplyViolationFee(fee);
            Debug.Log(message);
        }
    }
}
