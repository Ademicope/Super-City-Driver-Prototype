using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapRadar : MonoBehaviour
{
    public GameObject radarIndicator;  // Radar indicator to show on the mini-map
    public Transform carTransform;     // Reference to the car’s transform
    public Transform destinationTransform; // Reference to the destination point

    private RectTransform radarRectTransform;  // Reference to the radar's RectTransform

    private void Start()
    {
        radarRectTransform = radarIndicator.GetComponent<RectTransform>();
    }

    private void Update()
    {
        UpdateRadarPosition();
    }

    public void SetDestination(Transform destination)
    {
        destinationTransform = destination;
    }

    // Update the radar position based on the car's position
    private void UpdateRadarPosition()
    {
        if (carTransform == null || destinationTransform == null)
            return;

        // Convert car's position and destination position to a normalized direction vector
        Vector3 carPosition = carTransform.position;
        Vector3 destinationPosition = destinationTransform.position;

        Vector3 directionToDestination = destinationPosition - carPosition;
        directionToDestination.y = 0;  // Ignore height for the radar (2D)

        // Calculate the distance
        float distance = directionToDestination.magnitude;

        // Normalize the direction for the radar (scale it to fit within the mini-map)
        directionToDestination.Normalize();

        // Update the radar indicator's position relative to the car
        radarRectTransform.localPosition = new Vector3(directionToDestination.x * distance, directionToDestination.z * distance, 0);
    }
}
