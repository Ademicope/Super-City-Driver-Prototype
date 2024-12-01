using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAndEndPoint : MonoBehaviour
{
    public List<GameObject> startLocations = new List<GameObject>();
    public List<GameObject> destination = new List<GameObject>();

    public GameObject car;

    public bool isReachDestination = false;
    // Start is called before the first frame update
    void Start()
    {
        SetDestination();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetDestination()
    {
        if (startLocations.Count == 0 || destination.Count == 0)
        {
            Debug.LogError("start or destination is empty");
            return;
        }

        int startIndex = Random.Range(0, startLocations.Count);
        int destinationIndex = Random.Range(0, destination.Count);

        GameObject startPoint = startLocations[startIndex];
        GameObject destinationPoint = destination[destinationIndex];

        car.transform.position = startPoint.transform.position;
        car.transform.rotation = startPoint.transform.rotation;

    }
}
