using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartAndEndPoint : MonoBehaviour
{
    public List<GameObject> startLocations = new List<GameObject>();
    public List<GameObject> destination = new List<GameObject>();

    public bool isReachDestination = false;

    public GameManager gameManager;

    public GameObject carPointer;
    public GameObject destinationPointer;
    //public RectTransform miniMapRect;

    GameObject destinationPoint;

    // Start is called before the first frame update
    void Start()
    {
        SetDestination();
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePointerPositionsAndDirection();
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
            Debug.Log("destination reached");
            gameManager.GameWon();
        }
    }

    /*private void UpdatePointerPositions()
    {
        Vector2 carMiniMapPos = WorldToMiniMap(transform.position);
        Vector2 destMiniMapPos = WorldToMiniMap(destinationPoint.transform.position);

        carPointer.rectTransform.anchoredPosition = carMiniMapPos;
        destinationPointer.rectTransform.anchoredPosition = destMiniMapPos;

        // Rotate the destination pointer to point toward the destination
        Vector2 direction = destMiniMapPos - carMiniMapPos;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        destinationPointer.rectTransform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private Vector2 WorldToMiniMap(Vector3 worldPosition)
    {
        Vector3 localPosition = worldPosition - transform.position; // Relative to car
        float miniMapScale = miniMapRect.sizeDelta.x / 100f; // Adjust the scale
        return new Vector2(localPosition.x, localPosition.z) * miniMapScale;
    }*/

    private void UpdatePointerPositionsAndDirection()
    {
        // Position the car pointer slightly above the car for visibility
        carPointer.transform.position = transform.position + Vector3.up * 5f;

        // Position the destination pointer slightly above the destination point
        destinationPointer.transform.position = destinationPoint.transform.position + Vector3.up * 5f;

        // Calculate direction from the car to the destination
        Vector3 directionToDestination = destinationPoint.transform.position - transform.position;

        // Rotate the car pointer to point towards the destination
        if (directionToDestination != Vector3.zero) // Avoid errors if the direction is zero
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToDestination);
            carPointer.transform.rotation = Quaternion.Lerp(carPointer.transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }
}
