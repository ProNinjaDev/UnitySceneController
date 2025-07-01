using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionHandler : MonoBehaviour
{
    private static SelectionHandler _instance;

    [Header("UI Panel")]
    [SerializeField]
    private GameObject _uiPanel;

    [Header("Additional Toggle Elements")]
    [SerializeField]
    private GameObject[] _additionalToggleElements;

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

    public void ToggleUIPanel()
    {
        if (_uiPanel != null)
        {
            bool newState = !_uiPanel.activeSelf;
            
            _uiPanel.SetActive(newState);

            if (_additionalToggleElements != null)
            {
                foreach (var element in _additionalToggleElements)
                {
                    if (element != null)
                    {
                        element.SetActive(newState);
                    }
                }
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

    public void SetSelectionTransparency(float alpha)
    {
        if (selectedMat == null) return;

        Color currentColor = selectedMat.color;
        currentColor.a = alpha;
        selectedMat.color = currentColor;
    }

    public void ReplaceSelection(List<GameObject> newSelection)
    {
        foreach (var obj in _selectedObjects)
        {
            obj.GetComponent<Renderer>().material = defaultMat;
        }
        _selectedObjects.Clear();

        if (newSelection != null)
        {
            foreach (var obj in newSelection)
            {
                if (obj != null)
                {
                   _selectedObjects.Add(obj);
                   obj.GetComponent<Renderer>().material = selectedMat;
                }
            }
        }

        SelectionUpdated?.Invoke();
        Debug.Log($"Selection state loaded");
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
