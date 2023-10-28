using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;

public class RecenterPlayer : MonoBehaviour
{
    private InputDevice leftController;
    private InputDevice rightController;
    private bool recentered = false;

    private void Start()
    {
        // Get the left and right hand controllers
        var leftHandDevices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, leftHandDevices);
        leftController = leftHandDevices.FirstOrDefault();

        var rightHandDevices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, rightHandDevices);
        rightController = rightHandDevices.FirstOrDefault();
    }

    private void Update()
    {
        // Check for trigger button press on either controller
        if (!recentered && leftController.TryGetFeatureValue(CommonUsages.triggerButton, out bool leftTriggerPressed) && leftTriggerPressed)
        {
            RecenterPlayerPosition();
        }

        if (!recentered && rightController.TryGetFeatureValue(CommonUsages.triggerButton, out bool rightTriggerPressed) && rightTriggerPressed)
        {
            RecenterPlayerPosition();
        }
    }

    private void RecenterPlayerPosition()
    {
        // Recenter the player's position to the current position of the game object this script is attached to
        Vector3 newPosition = transform.position;
        XRDevice.SetTrackingSpaceType(TrackingSpaceType.Stationary);
        InputTracking.Recenter();
        XRDevice.SetTrackingSpaceType(TrackingSpaceType.RoomScale);
        transform.position = newPosition;
        recentered = true; // Set the flag to indicate that recentering has occurred
    }
}
