using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    public float Speed;

    private void Update()
    {
        transform.position += Vector3.forward * Speed * Time.deltaTime;
    }
}
