using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] WheelCollider frontRight;
    [SerializeField] WheelCollider backRight;
    [SerializeField] WheelCollider frontLeft;
    [SerializeField] WheelCollider backLeft;

    [SerializeField] Transform frontRightTransform;
    [SerializeField] Transform backRightTransform;
    [SerializeField] Transform frontLeftTransform;
    [SerializeField] Transform backLeftTransform;

    private float verticalInput, horizontalInput;

    public float maxMotorTorque = 300f;
    //public float acceleration = 300f;
    public float acceleration = 0f;
    public float brakingForce = 150f;
    public float maxTurnAngle = 30f;
    public float accelerationRate = 0f;
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
    }

    public void GetSpeed()
    {
        Vector3 localVelocity = transform.InverseTransformDirection(carRb.velocity);
        float forwardSpeed = localVelocity.z;
        currentSpeed = Mathf.Max(forwardSpeed * 3.6f); // Convert m/s to km/h
        Debug.Log("Current Speed: " + currentSpeed + " km/h");

        if (verticalInput > 0 && currentSpeed < maxSpeed)
        {
            //currentSpeed += accelerationRate * Time.deltaTime;

            // Calculate the motor torque based on current speed
            motorTorque = Mathf.Clamp(verticalInput * maxMotorTorque, 0, maxMotorTorque);
            // Apply motor torque to the rear wheels
            backRight.motorTorque = motorTorque;
            backLeft.motorTorque = motorTorque;
        }
        else// if (verticalInput == 0 || currentSpeed > maxSpeed)
        {
            motorTorque = 0;
            backRight.motorTorque = motorTorque;
            backLeft.motorTorque = motorTorque;
            //currentSpeed = Mathf.Max(currentSpeed - (accelerationRate * 0.5f * Time.deltaTime), 0);

        }

        // Simulate rolling friction by reducing speed when no input is given
        ApplyRollingFriction();
    }

    private void ApplyRollingFriction()
    {
        if (carRb.velocity.magnitude > 0)
        {
            float rollingResistance = 10f; // Arbitrary value for resistance
            Vector3 resistanceForce = -carRb.velocity.normalized * rollingResistance;
            //currentSpeed = Mathf.Max(0f, currentSpeed - rollingResistance * Time.deltaTime);
            carRb.AddForce(resistanceForce, ForceMode.Acceleration);
        }
    }

    void FixedUpdate()
    {
        GetSpeed();
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
        ApplyBrakes(brakingForce);

        // Handle steering
        HandleSteering();

        //update wheel meshes
        UpdateWheel(frontRight, frontRightTransform);
        UpdateWheel(frontLeft, frontLeftTransform);
        UpdateWheel(backRight, backRightTransform);
        UpdateWheel(backLeft, backLeftTransform);
    }

    private void ApplyBrakes(float brakeForce)
    {
        frontRight.brakeTorque = brakeForce;
        frontLeft.brakeTorque = brakeForce;
        backRight.brakeTorque = brakeForce;
        backLeft.brakeTorque = brakeForce;
    }

    private void HandleSteering()
    {
        currentTurnAngle = maxTurnAngle * horizontalInput;
        frontRight.steerAngle = currentTurnAngle;
        frontLeft.steerAngle = currentTurnAngle;
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

    //Button controls
    public void Accelerate()
    {
        Debug.Log("Accelerating");
        verticalInput = 1f;
        accelerationRate = 5f;
        acceleration = 300f;
    }

    public void StopAccelerate()
    {
        Debug.Log("Accelerating");
        verticalInput = 0f;
        accelerationRate = 0f;
        acceleration = 0f;
    }

}
