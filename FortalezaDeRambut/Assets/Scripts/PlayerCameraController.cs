using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    float yaw;
    float pitch = 2f;

    public Transform target;

    public float mouseSensitivity = 6;
    public float minimumPitchAngule = 0;
    public float maximumPitchAngule = 55;
    
    // Se vuelve mas sueve el movimiento de la camara
    public float rotationSoomthTime = 0.08f;
    Vector3 rotationSmoothVelocity;
    Vector3 currentRotation;

    public float zoomSpeed = 4f;
    public float minZoom = 3f;
    public float maxZoom = 15f;
    private float currentZoom = 10f;

    private bool cameraControl = false;

    void Start()
    {
        transform.position = target.position - transform.forward * currentZoom;
    }

    void Update()
    {
        currentZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
    }

    void LateUpdate()
    {

        if (Input.GetMouseButtonDown(2) || cameraControl)
        {
            cameraControl = true;

            yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
            pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;

            // Se restringe el maximo y minimo de angulo de la camara en el eje Y
            pitch = Mathf.Clamp(pitch, minimumPitchAngule, maximumPitchAngule);
        }

        if (Input.GetMouseButtonUp(2))
        {
            cameraControl = false;
        }

        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationSmoothVelocity, rotationSoomthTime);
        transform.eulerAngles = currentRotation;

        transform.position = target.position - transform.forward * currentZoom;

    }
}
