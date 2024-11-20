using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CAM: MonoBehaviour
{
    // Reference to the Oculus CenterEyeAnchor for head rotation
    public Transform centerEyeAnchor;

    // Update is called once per frame
    void Update()
    {
        if (centerEyeAnchor != null)
        {
            // Get the headset's rotation
            Quaternion headRotation = centerEyeAnchor.rotation;

            // Apply the rotation to the camera (or the object this script is attached to)
            transform.rotation = headRotation;
        }
    }
}