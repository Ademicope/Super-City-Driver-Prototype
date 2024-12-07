using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
    public float brakingForce = 300f;
    public float maxTurnAngle = 90f;
    //public float accelerationRate = 0f;
    public float maxSpeed = 220f;

    private float currentAcceleration = 0;
    private float currentBrakeForce = 0;
    private float currentTurnAngle = 0;

    public float currentSpeed = 0;
    //private float motorTorque = 0f;

    public MyButton acceleratePedal;
    public MyButton reversePedal;
    public MyButton brakePedal;
    public MyButton leftButton;
    public MyButton rightButton;

    

    //public float accelerateInput;
    public float brakeInput;

    private Rigidbody carRb;
    //public TrafficRuleTrigger trafficRuleTrigger;
    //public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        carRb = GetComponent<Rigidbody>();
        carRb.velocity = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetSpeed()
    {
        //Debug.Log("Rigidbody Velocity: " + carRb.velocity);
        Vector3 localVelocity = transform.InverseTransformDirection(carRb.velocity);
        float forwardSpeed = localVelocity.z;
        if (Mathf.Abs(forwardSpeed) > 0.1f)
        {
            currentSpeed = forwardSpeed * 3.6f; // Convert m/s to km/h
            
        }
        else
        {
            currentSpeed = 0;
        }

        // Simulate rolling friction by reducing speed when no input is given
        ApplyRollingFriction();
    }

    

    private void ApplyRollingFriction()
    {
        if (currentBrakeForce == 0 && carRb.velocity.magnitude > 0.1f)
        {
            float rollingResistance = 1f; // Arbitrary value for resistance
            Vector3 resistanceForce = -carRb.velocity.normalized * rollingResistance;
            carRb.AddForce(resistanceForce, ForceMode.Acceleration);
        }
    }

    void FixedUpdate()
    {
        GetSpeed();
        GetInput();

        if (brakePedal.isPressed || Input.GetKey(KeyCode.Space))
        {
            ApplyBrakes();
        }
        else if (Mathf.Abs(verticalInput) > 0.01f)
        {
            // Get acceleration and decceleration from vertical input
            currentAcceleration = acceleration * verticalInput;

            //Apply acceleration to rear wheels & clamp motor torque
            if (currentSpeed < maxSpeed || verticalInput < 0)
            {
                backRight.motorTorque = currentAcceleration;
                backLeft.motorTorque = currentAcceleration;
                currentBrakeForce = 0;
            }
            else
            {
                backRight.motorTorque = 0;
                backLeft.motorTorque = 0;
            }
            // Clear brake torque
            frontRight.brakeTorque = 0;
            frontLeft.brakeTorque = 0;
            backRight.brakeTorque = 0;
            backLeft.brakeTorque = 0;
        }
        else
        {
            //Apply brake to all wheels
            ApplyBrakes();
        }

        // Handle steering
        HandleSteering();

        //update wheel meshes
        UpdateWheelMeshes();
    }

    private void GetInput()
    {
        verticalInput = Input.GetAxis("Vertical");
        if (acceleratePedal.isPressed)
        {
            verticalInput += acceleratePedal.dampenPress;
        }
        if (reversePedal.isPressed)
        {
            verticalInput -= reversePedal.dampenPress;
        }

        horizontalInput = Input.GetAxis("Horizontal");
        if (rightButton.isPressed)
        {
            horizontalInput += rightButton.dampenPress;
        }
        if (leftButton.isPressed)
        {
            horizontalInput -= leftButton.dampenPress;
        }

        //reversing
        float movingDirection = Vector3.Dot(transform.forward, carRb.velocity);
        if (movingDirection < -0.5f && verticalInput > 0)
        {
            brakeInput = Mathf.Abs(verticalInput);
        }
        else if (movingDirection > 0.5f && verticalInput < 0)
        {
            brakeInput = Mathf.Abs(verticalInput);
        }
        else
        {
            brakeInput = 0;
        }
    }

    private void ApplyBrakes()
    {
        currentBrakeForce = brakingForce * 2;

        frontRight.brakeTorque = currentBrakeForce;
        frontLeft.brakeTorque = currentBrakeForce;
        backRight.brakeTorque = currentBrakeForce;
        backLeft.brakeTorque = currentBrakeForce;

        if (carRb.velocity.magnitude < 0.1f && currentBrakeForce > 0)
        {
            carRb.velocity = Vector3.zero;
            carRb.angularVelocity = Vector3.zero;
        }

        // Ensure no motor torque while braking
        backRight.motorTorque = 0;
        backLeft.motorTorque = 0;
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

}
