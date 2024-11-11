using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject car;

    public Vector3 offset = new Vector3 (0, 0, 0);
    private Vector3 initialAngle, finalAngle;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        
        transform.position = car.transform.position + offset;
        transform.rotation = car.transform.rotation;
    }
}
