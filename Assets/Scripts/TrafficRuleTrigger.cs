using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficRuleTrigger : MonoBehaviour
{
    public enum Rule { SpeedCheck, PedestrianCrossing }

    public Rule rule;

    public float maxAllowedSpeed = 50f;
    public float stopDuration = 3f;

    public float violationFee = 0;

    CarController car;

    public GameObject goLight;

    private void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            car = other.GetComponent<CarController>();
            Debug.Log("Player entered the trigger");
            if (rule == Rule.PedestrianCrossing && (goLight != null && !goLight.activeSelf))
            {
                violationFee += 15;
                Debug.Log("Traffic light violation!! lost 10 coins");
                NotifyGameManager(violationFee, "Traffic light violation!! lost 10 coins");
            }
            if (rule == Rule.SpeedCheck)
            {
                if (car != null && car.currentSpeed > maxAllowedSpeed)
                {
                    violationFee += 15;
                    Debug.Log("Speed violation!! lost 5 coins");
                }
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
