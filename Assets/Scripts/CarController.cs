using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    //public CarController carController;

    public Transform steeringWheel;
    public Transform frontRightWheel, frontLeftWheel;

    private Rigidbody rbCar;

    public float speed = 10.0f;
    public float carSpeed;
    public float rotateSpeed = 50.0f;
    private float forwardInput, sideInput;

    public float maxSteeringAngle = 30f;
    public float maxWheelRotation = 120f;


    private Vector3 lastPos;
    // Start is called before the first frame update
    void Start()
    {
        rbCar = GetComponent<Rigidbody>();
        lastPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
        //wheelDirection = frontLeftWheel.transform.forward;
        sideInput = Input.GetAxis("Horizontal");
        forwardInput = Input.GetAxis("Vertical");

        float steeringWheelRotatation = sideInput * maxWheelRotation;
        steeringWheel.localEulerAngles = new Vector3(0, 0, -steeringWheelRotatation);

        float wheelRotation = sideInput * maxSteeringAngle * 0.5f;
        frontLeftWheel.localEulerAngles = new Vector3(0, wheelRotation, 0);
        frontRightWheel.localEulerAngles = new Vector3(0, wheelRotation, 0);

        Vector3 forwardDirection = Quaternion.Euler(0, wheelRotation, 0) * transform.forward;
        //transform.position += forwardDirection * speed * Time.deltaTime;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(forwardDirection), rotateSpeed * Time.deltaTime);
        transform.Translate(Vector3.forward * Time.deltaTime * speed * forwardInput);
        rbCar.AddForce(transform.forward * speed * forwardInput, ForceMode.Acceleration);
        UpdateSpeed();
    }

    private void FixedUpdate()
    {
        //UpdateSpeed();
    }

    public void UpdateSpeed()
    {
        //carSpeed = rbCar.velocity.magnitude;
        carSpeed = (transform.position - lastPos).magnitude / Time.deltaTime;
        lastPos = transform.position;
    }
}
