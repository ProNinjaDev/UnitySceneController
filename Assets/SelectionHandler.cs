using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionHandler : MonoBehaviour
{
    public Material defaultMat;
    public Material selectedMat;

    private GameObject currentSelectedObject;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Interactable"))
                {
                    HandleSelection(hit.collider.gameObject);
                }
                else
                {
                    HandleDeselection();
                }
            }
            else
            {
                HandleDeselection();
            }
        }
    }

    void HandleSelection(GameObject newObject)
    {
        if (currentSelectedObject != null)
        {
            currentSelectedObject.GetComponent<Renderer>().material = defaultMat;
        }

        currentSelectedObject = newObject;
        currentSelectedObject.GetComponent<Renderer>().material = selectedMat;
        Debug.Log($"Selected object: {currentSelectedObject.name}");
    }

    void HandleDeselection()
    {
        if (currentSelectedObject != null)
        {
            currentSelectedObject.GetComponent<Renderer>().material = defaultMat;
            currentSelectedObject = null;
            Debug.Log("Selection cleared.");
        }
    }
}
