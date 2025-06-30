using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionHandler : MonoBehaviour
{
    private static SelectionHandler _instance;

    [Header("Materials")]
    public Material defaultMat;
    public Material selectedMat;


    [Header("Dependencies")]
    public CameraController cameraController;

    private readonly List<GameObject> _selectedObjects = new List<GameObject>();

    public event Action SelectionUpdated;


    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            
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
            SelectionUpdated?.Invoke();
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
        SelectionUpdated?.Invoke();
    }

    public bool IsSelected(GameObject obj)
    {
        return _selectedObjects.Contains(obj);
    }

    public static SelectionHandler GetInstance() => _instance;

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
