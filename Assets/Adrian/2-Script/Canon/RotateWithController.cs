using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWithController : MonoBehaviour
{
    public GameObject controller; // The object to follow

    private Quaternion initialRotationOffset;

    private void Start()
    {
        if (controller != null)
        {
            // Calculate the initial rotation offset between the object and the controller
            Quaternion inverseControllerRotation = Quaternion.Inverse(controller.transform.rotation);
            initialRotationOffset = Quaternion.Euler(35f, 0f, 0f) * inverseControllerRotation * transform.rotation;
        }
    }

    private void LateUpdate()
    {
        if (controller != null)
        {
            // Calculate the target rotation based on the controller's rotation and the initial offset
            Quaternion targetRotation = controller.transform.rotation * initialRotationOffset;

            // Apply the rotation to the object
            transform.rotation = targetRotation;
        }
    }
}
