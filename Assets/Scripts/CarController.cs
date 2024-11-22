using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField]
    WheelCollider frontRight;
    [SerializeField]
    WheelCollider backRight;
    [SerializeField]
    WheelCollider frontLeft;
    [SerializeField]
    WheelCollider backLeft;

    [SerializeField]
    Transform frontRightTransform;
    [SerializeField]
    Transform backRightTransform;
    [SerializeField]
    Transform frontLeftTransform;
    [SerializeField]
    Transform backLeftTransform;

    private float verticalInput, horizontalInput;

    public float maxMotorTorque = 300f;
    public float acceleration = 300f;
    public float brakingForce = 150f;
    public float maxTurnAngle = 30f;
    public float accelerationRate = 5f;
    public float maxSpeed = 220f;

    private float currentAcceleration = 0;
    private float currentBrakeForce = 0;
    private float currentTurnAngle = 0;

    public float currentSpeed = 0;
    private float motorTorque = 0f;

    private Rigidbody carRb;

    // Start is called before the first frame update
    void Start()
    {
        carRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");

        if (verticalInput > 0 && currentSpeed < maxSpeed)
        {
            currentSpeed += accelerationRate * Time.deltaTime;
        }
        else if (verticalInput == 0 || currentSpeed > maxSpeed)
        {
            currentSpeed = Mathf.Max(currentSpeed -(accelerationRate * 0.5f * Time.deltaTime), 0);

        }
        // Convert speed from km/h to m/s for motor torque
        float speedInMetersPerSecond = currentSpeed / 3.6f;

        // Calculate the motor torque based on current speed
        motorTorque = Mathf.Clamp(verticalInput * maxMotorTorque, 0, maxMotorTorque);

        // Apply motor torque to the rear wheels
        backRight.motorTorque = motorTorque;
        backLeft.motorTorque = motorTorque;

        // Simulate rolling friction by reducing speed when no input is given
        ApplyRollingFriction();
    }

    private void ApplyRollingFriction()
    {
        float rollingResistance = 10f; // Arbitrary value for resistance
        currentSpeed = Mathf.Max(0f, currentSpeed - rollingResistance * Time.deltaTime);
    }

    void FixedUpdate()
    {
        // Get acceleration and decceleration from vertical input
        currentAcceleration = acceleration * verticalInput;

        if (Input.GetKey(KeyCode.Space))
        {
            currentBrakeForce = brakingForce;
        }
        else
        {
            currentBrakeForce = 0;
        }

        //Apply acceleration to front wheels
        frontRight.motorTorque = currentAcceleration;
        frontLeft.motorTorque = currentAcceleration;

        //Apply brake to all wheels
        frontRight.brakeTorque = currentBrakeForce;
        frontLeft.brakeTorque = currentBrakeForce;
        backRight.brakeTorque = currentBrakeForce;
        backLeft.brakeTorque = currentBrakeForce;

        // Handle steering
        currentTurnAngle = maxTurnAngle * Input.GetAxis("Horizontal");
        frontRight.steerAngle = currentTurnAngle;
        frontLeft.steerAngle = currentTurnAngle;

        //update wheel meshes
        UpdateWheel(frontRight, frontRightTransform);
        UpdateWheel(frontLeft, frontLeftTransform);
        UpdateWheel(backRight, backRightTransform);
        UpdateWheel(backLeft, backLeftTransform);

        GetSpeed();
    }

    private void UpdateWheel(WheelCollider wheel, Transform trans)
    {
        //Get wheel state
        Vector3 position;
        Quaternion rotation;

        wheel.GetWorldPose(out position, out rotation);

        //set wheel transform
        trans.position = position;
        trans.rotation = rotation;
    }

    public void GetSpeed()
    {
        // Calculate and display the speed in km/h
        Vector3 localVelocity = transform.InverseTransformDirection(carRb.velocity);
        float forwardSpeed = localVelocity.z;
        currentSpeed = forwardSpeed * 3.6f; // Convert m/s to km/h

        // Visual debugging (optional)
        Debug.Log("Current Speed: " + currentSpeed + " km/h");
        //currentSpeed =  carRb.velocity.magnitude;
    }
    
}
