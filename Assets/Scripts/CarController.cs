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

    private float verticalInput = 0;
    private float horizontalInput = 0;

    public float maxMotorTorque = 2000f;
    //public float acceleration = 300f;
    public float acceleration = 1500f;
    public float brakingForce = 150f;
    public float maxTurnAngle = 30f;
    //public float accelerationRate = 0f;
    public float maxSpeed = 220f;

    private float currentAcceleration = 0;
    private float currentBrakeForce = 0;
    private float currentTurnAngle = 0;

    public float currentSpeed = 0;
    //private float motorTorque = 0f;

    private bool isUsingUIBotton = false;

    private Rigidbody carRb;

    // Start is called before the first frame update
    void Start()
    {
        carRb = GetComponent<Rigidbody>();
        carRb.velocity = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isUsingUIBotton)
        {
            verticalInput = Input.GetAxis("Vertical");
            horizontalInput = Input.GetAxis("Horizontal");
        }
    }

    public void GetSpeed()
    {
        Debug.Log("Rigidbody Velocity: " + carRb.velocity);
        Vector3 localVelocity = transform.InverseTransformDirection(carRb.velocity);
        float forwardSpeed = localVelocity.z;
        if (forwardSpeed > 0)
        {
            currentSpeed = forwardSpeed * 3.6f; // Convert m/s to km/h
        }
        else
        {
            currentSpeed = 0;
        }
        //Debug.Log("Current Speed: " + currentSpeed + " km/h");

        /*if (verticalInput > 0 && currentSpeed < maxSpeed)
        {
            Debug.Log($"Vertical Input: {verticalInput}");
            //currentSpeed += accelerationRate * Time.deltaTime;

            // Calculate the motor torque based on current speed
            motorTorque = Mathf.Clamp(verticalInput * maxMotorTorque, 0, maxMotorTorque);
            // Apply motor torque to the rear wheels
            backRight.motorTorque = motorTorque;
            backLeft.motorTorque = motorTorque;
            
        }
        else
        {
            backRight.motorTorque = 0;
            backLeft.motorTorque = 0;
        }*/

        // Simulate rolling friction by reducing speed when no input is given
        ApplyRollingFriction();
    }

    private void ApplyRollingFriction()
    {
        if (carRb.velocity.magnitude > 0)
        {
            float rollingResistance = 0.1f; // Arbitrary value for resistance
            Vector3 resistanceForce = -carRb.velocity.normalized * rollingResistance;
            carRb.AddForce(resistanceForce, ForceMode.Acceleration);
        }
    }

    void FixedUpdate()
    {
        GetSpeed();

        if (verticalInput > 0)
        {
            // Get acceleration and decceleration from vertical input
            currentAcceleration = acceleration * verticalInput;

            //Apply acceleration to rear wheels
            backRight.motorTorque = currentAcceleration;
            backLeft.motorTorque = currentAcceleration;
        }
        else
        {
            //backRight.motorTorque = 0;
            //backLeft.motorTorque = 0;

            //Apply brake to all wheels
            ApplyBrakes(currentBrakeForce);
        }
        

        // Handle steering
        HandleSteering();

        //update wheel meshes
        UpdateWheelMeshes();
    }

    private void ApplyBrakes(float brakeForce)
    {
        if (Input.GetKey(KeyCode.Space) || verticalInput < 0)
        {
            currentBrakeForce = brakingForce;
        }
        else
        {
            currentBrakeForce = 0;
        }

        frontRight.brakeTorque = currentBrakeForce;
        frontLeft.brakeTorque = currentBrakeForce;
        backRight.brakeTorque = currentBrakeForce;
        backLeft.brakeTorque = currentBrakeForce;
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

    private void UpdateWheelMeshes()
    {
        UpdateWheel(frontRight, frontRightTransform);
        UpdateWheel(frontLeft, frontLeftTransform);
        UpdateWheel(backRight, backRightTransform);
        UpdateWheel(backLeft, backLeftTransform);
    }

    #region Button controls
    //Button controls
    public void AccelerateButtonDown()
    {
        isUsingUIBotton = true;
        Debug.Log("Accelerating");
        verticalInput = 1f;
    }

    public void AccelerateButtonup()
    {
        isUsingUIBotton = true;
        Debug.Log("Stopping");
        verticalInput = 0f;
    }
    
    public void BrakeButtonDown()
    {
        isUsingUIBotton = true;
        verticalInput = -1f;
    }

    public void BrakeButtonUp()
    {
        isUsingUIBotton = true;
        verticalInput = 0f;
    }

    public void RightButtonDown()
    {
        isUsingUIBotton = true;
        horizontalInput = 1f;
    }

    public void LeftButtonDown()
    {
        isUsingUIBotton = true;
        horizontalInput = -1f;
    }

    public void RightButtonUp()
    {
        isUsingUIBotton = true;
        horizontalInput = 0f;
    }

    public void LeftButtonUp()
    {
        isUsingUIBotton = true;
        horizontalInput = 0f;
    }
    #endregion
}
