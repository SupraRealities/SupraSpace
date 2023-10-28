using UnityEngine;

public class RotatorAccordingToCalibration : MonoBehaviour
{
    private void Start()
    {
        transform.Rotate(Vector3.up * SharedData.Instance.calibrationAngleDifference);
    }
}
