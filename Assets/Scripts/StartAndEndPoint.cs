using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAndEndPoint : MonoBehaviour
{
    public List<GameObject> startLocations = new List<GameObject>();
    public List<GameObject> destination = new List<GameObject>();

    public bool isReachDestination = false;

    public GameManager gameManager;

    GameObject destinationPoint;

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
        destinationPoint = destination[destinationIndex];

        transform.position = startPoint.transform.position;
        transform.rotation = startPoint.transform.rotation;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == destinationPoint)
        {
            isReachDestination = true;
            gameManager.GameWon();
        }
    }
}
