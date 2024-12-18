using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public bool isButtonPressed = false;

    public void AccelerateButtonDown()
    {
        isButtonPressed = true;
        Debug.Log("Accelerating");
    }

    public void AccelerateButtonup()
    {
        isButtonPressed = false;
        Debug.Log("Stopping");
    }

    public void BrakeButtonDown()
    {
        isButtonPressed = true;
    }

    public void BrakeButtonUp()
    {
        isButtonPressed = false;
    }

    public void RightButtonDown()
    {
        isButtonPressed = true;
    }

    public void LeftButtonDown()
    {
        isButtonPressed = true;
    }

    public void RightButtonUp()
    {
        isButtonPressed = false;
    }

    public void LeftButtonUp()
    {
        isButtonPressed = false;
    }
}
