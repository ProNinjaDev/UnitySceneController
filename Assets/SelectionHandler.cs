using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionHandler : MonoBehaviour
{
    private static SelectionHandler _instance;

    [Header("Materials")]
    public Material defaultMat;
    public Material selectedMat;

    //private GameObject currentSelectedObject;

    [Header("Dependencies")]
    public CameraController cameraController;

    private readonly List<GameObject> _selectedObjects = new List<GameObject>();

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit))
            {
                if (hit.collider.CompareTag("Interactable"))
                {
                    HandleSelection(hit.collider.gameObject, true);
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

    public void HandleSelection(GameObject selectedObject, bool focusCamera = false)
    {
        if (selectedObject == null) return;

        if (_selectedObjects.Contains(selectedObject))
        {
            HandleDeselection(selectedObject);
        }
        else
        {
            _selectedObjects.Add(selectedObject);
            selectedObject.GetComponent<Renderer>().material = selectedMat;
            Debug.Log($"Selected object: {selectedObject.name}");

            if (focusCamera)
            {
                cameraController.FocusOnObject(selectedObject.transform);
            }
        }
    }

    public void HandleDeselection()
    {
        foreach (var obj in new List<GameObject>(_selectedObjects))
        {
            HandleDeselection(obj);
        }
        Debug.Log("Selection cleared");
    }

    public void HandleDeselection(GameObject selectedObject)
    {
        if (selectedObject == null || !_selectedObjects.Contains(selectedObject)) return;

        selectedObject.GetComponent<Renderer>().material = defaultMat;
        _selectedObjects.Remove(selectedObject);
    }

    public static SelectionHandler GetInstance()
    {
        return _instance;
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }
}
