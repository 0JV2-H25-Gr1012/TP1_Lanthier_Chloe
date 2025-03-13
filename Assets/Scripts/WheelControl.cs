using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelControl : MonoBehaviour
{
    public GameObject model;
    Vector3 position;
    Quaternion rotation;
    public WheelCollider wc;
    public bool steerable;
    public bool motorized;
    public float offsetRotationY = 180f;
    public Vector3 offsetPosition;

    // Start is called before the first frame update
    void Start()
    {
        wc = GetComponent<WheelCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get the Wheel collider's world pose values and
        // use them to set the wheel model's position and rotation
        wc.GetWorldPose(out position, out rotation);
        model.transform.position = position + model.transform.TransformDirection(offsetPosition);
        model.transform.rotation = rotation;
        model.transform.Rotate(0, offsetRotationY, 0, Space.Self);
    }
}
