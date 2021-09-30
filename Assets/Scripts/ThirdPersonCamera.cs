using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform targetLookTransform;
    
    public float distance = 5f; // start distance for camera
    public float distanceMin = 3f; // min distance for camera
    public float distanceMax = 10f; // max distance for camera
    public float distanceSmooth = 0.05f; // camera zooming smoothing factor

    public float mouseXSensitivity = 5f;
    public float mouseYSensitivity = 5f;
    public float mouseWheelSensitivity = 5f;

    public float xSmooth = 0.05f;                   // Smoothness factor for x position calculations.
    public float ySmooth = 0.1f;                    // Smoothness factor for y position calculations.
    public float yMinLimit = -40f;                  
    public float yMaxLimit = 80f;                   
    private float mouseX = 0f;
    private float mouseY = 0f;
    private float velocityX = 0f; 
    private float velocityY = 0f;
    private float velocityZ = 0f;
    private float velocityDistance = 0f;

    private float startDistance = 0f;
    private float desiredDistance = 0f;
 
    private Vector3 position;
    private Vector3 desiredPosition;

    private void Start()
    {
        distance = Mathf.Clamp(distance, distanceMin, distanceMax);
        startDistance = distance;
        Reset();
    }

    private void LateUpdate()
    {
        HandleInput();
        CalculateDesiredPosition();
        UpdatePosition();
    }

    private void HandleInput()
    {
        float deadZone = 0.01f;

        mouseX += Input.GetAxis("Mouse X") * mouseXSensitivity;
        mouseY -= Input.GetAxis("Mouse Y") * mouseYSensitivity;

        mouseY = ClampAngle(mouseY, yMinLimit, yMaxLimit);

        var scrollWheel = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scrollWheel) > deadZone)
        {
            desiredDistance = Mathf.Clamp(distance - scrollWheel * mouseWheelSensitivity, distanceMin, distanceMax);
        }
    }

    private void CalculateDesiredPosition()
    {
        distance = Mathf.SmoothDamp(distance, desiredDistance, ref velocityDistance, distanceSmooth);
        desiredPosition = CalculatePosition(mouseY, mouseX, distance);
    }

    private Vector3 CalculatePosition(float rotX, float rotY, float rotDist)
    {
        var direction = new Vector3(0, 0, -distance);          // -distance because we want it to point behind our character.
        var rotation = Quaternion.Euler(rotX, rotY, 0);
        return targetLookTransform.position + (rotation * direction);
    }

    private void UpdatePosition ()
    {
        float posX = Mathf.SmoothDamp(position.x, desiredPosition.x, ref velocityX, xSmooth * Time.deltaTime);
        float posY = Mathf.SmoothDamp(position.y, desiredPosition.y, ref velocityY, ySmooth * Time.deltaTime);
        float posZ = Mathf.SmoothDamp(position.z, desiredPosition.z, ref velocityZ, xSmooth * Time.deltaTime);
 
        position = new Vector3(posX, posY, posZ);
 
        transform.position = position;
 
        transform.LookAt(targetLookTransform);
    }

    public void Reset ()
    {
        mouseX = 0f;
        mouseY = 10f;
        distance = startDistance;
        desiredDistance = distance;
    }

    private static float ClampAngle(float angle, float min, float max)
    {
        while (angle < -360) angle += 360;
        while (angle > 360) angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
}
