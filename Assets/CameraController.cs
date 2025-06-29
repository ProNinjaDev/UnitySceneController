using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform currentTarget;
    public Vector3 offset = new Vector3(0f, 3f, -5f);
    public float smoothSpeed = 5f;

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
