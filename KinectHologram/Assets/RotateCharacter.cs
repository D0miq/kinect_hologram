using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCharacter : MonoBehaviour
{
    public Transform character;

    float degreesPerSecond = 50.0f;

    void Update()
    {
        transform.Rotate(Vector3.forward * degreesPerSecond * Time.deltaTime, Space.Self);
    }
}
