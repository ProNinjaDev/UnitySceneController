using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform currentTarget;
    private Vector3 focusPoint;

    [Header("Settings")]
    public Vector3 offset = new Vector3(0f, 3f, -5f);
    public float smoothSpeed = 3f;
    public float rotationSpeed = 5f;
    public float zoomSpeed = 10f;
    public float panSpeed = 2f;
    public float smoothPan = 0.01f;

    [Header("Zoom limits")]
    public float minZoomDistance = 2f;
    public float maxZoomDistance = 15f;


    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void LateUpdate()
    {
        if (currentTarget != null) 
        {

            float scrollInput = Input.GetAxis("Mouse ScrollWheel");

            if (scrollInput != 0f) 
            {
                float currentDistance = offset.magnitude;
                float newDistance = currentDistance - scrollInput * zoomSpeed;

                newDistance = Mathf.Clamp(newDistance, minZoomDistance, maxZoomDistance);

                offset = offset.normalized * newDistance;

            }

            if (Input.GetMouseButton(2))
            {
                float currentDistance = offset.magnitude;
                focusPoint -= transform.right * Input.GetAxis("Mouse X") * panSpeed * currentDistance * smoothPan;
                focusPoint -= transform.up * Input.GetAxis("Mouse Y") * panSpeed * currentDistance * smoothPan;
            }


            else if (Input.GetMouseButton(1))
            {
                float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
                offset = Quaternion.AngleAxis(mouseX, Vector3.up) * offset;

                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    float mouseY = -Input.GetAxis("Mouse Y") * rotationSpeed;
                    Vector3 oldOffset = offset;
                    offset = Quaternion.AngleAxis(mouseY, transform.right) * offset;

                    float angle = Vector3.Angle(Vector3.up, offset);
                    if (angle < 5f || angle > 85f)
                    {
                        offset = oldOffset;
                    }
                }
            }

            Vector3 targetCameraPos = focusPoint + offset;
            Vector3 newPosition = Vector3.Lerp(transform.position, targetCameraPos, smoothSpeed * Time.deltaTime);
            transform.position = newPosition;

            transform.LookAt(focusPoint);
        }
    }

    public void FocusOnObject(Transform focusTarget) 
    {
        currentTarget = focusTarget;
        focusPoint = currentTarget.position;
    }
}
