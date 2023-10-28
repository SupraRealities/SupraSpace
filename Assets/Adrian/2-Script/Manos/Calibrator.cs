using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calibrator : MonoBehaviour
{
    [SerializeField] private Transform xrOrigin;
    [SerializeField] private Transform perfectTransform;

    private float minRotationY = 0;
    private float maxRotationY = 0;

    private bool viewportResetted;

    private void Update()
    {
        float currentRotationY = transform.eulerAngles.y;
        currentRotationY = (currentRotationY > 180) ? currentRotationY - 360 : currentRotationY;
        if (currentRotationY < minRotationY)
        {
            minRotationY = currentRotationY;
        }
        if (currentRotationY > maxRotationY)
        {
            maxRotationY = currentRotationY;
        }

        float rotationYRange = Mathf.Abs(maxRotationY - minRotationY);
        if (rotationYRange >= 65f)
        {
            ResetViewport();
        }
    }

    private void ResetViewport()
    {
        if (viewportResetted)
        {
            return;
        }
        viewportResetted = true;

        float midRotationY = (maxRotationY + minRotationY) / 2;

        float angleDifference = perfectTransform.eulerAngles.y - midRotationY;

        xrOrigin.Rotate(Vector3.up * angleDifference);
        SharedData.Instance.calibrationAngleDifference = angleDifference;
    }
}
