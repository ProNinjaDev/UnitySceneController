using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform currentTarget;
    public Vector3 offset = new Vector3(0f, 3f, -5f);
    public float smoothSpeed = 3f;
    public float rotationSpeed = 5f;
    public float zoomSpeed = 10f;
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


            if (Input.GetMouseButton(1))
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

            Vector3 targetCameraPos = currentTarget.position + offset;
            Vector3 newPosition = Vector3.Lerp(transform.position, targetCameraPos, smoothSpeed * Time.deltaTime);
            transform.position = newPosition;

            transform.LookAt(currentTarget);
        }
    }

    public void FocusOnObject(Transform focusTarget) 
    {
        currentTarget = focusTarget;
        //Debug.Log($"Focus on {focusTarget.name}");
    }
}
