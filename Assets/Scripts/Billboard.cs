using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform mainCameraTransform;

    void LateUpdate()
    {
        Debug.Log(Camera.main);
        Debug.Log(mainCameraTransform);
        if (Camera.main == null) return;

        if (mainCameraTransform == null)
        {
            mainCameraTransform = Camera.main.transform;
        }

        transform.LookAt(transform.position + mainCameraTransform.rotation * Vector3.forward,
            mainCameraTransform.rotation * Vector3.up);
    }
}
