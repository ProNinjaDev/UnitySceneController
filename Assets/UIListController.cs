using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIListController : MonoBehaviour
{
    [SerializeField]
    private GameObject lineOfListPrefab;

    [SerializeField]
    private Transform listContainer; 

    private readonly List<GameObject> _interactableObjects = new List<GameObject>();
    private bool _allObjectsVisible = true;


    void Start()
    {
        foreach (Transform child in listContainer)
        {
            Destroy(child.gameObject);
        }

        _interactableObjects.AddRange(GameObject.FindGameObjectsWithTag("Interactable"));

        foreach (var obj in _interactableObjects)
        {
            GameObject newLine = Instantiate(lineOfListPrefab, listContainer);

            UIListItem itemScript = newLine.GetComponent<UIListItem>();
            if (itemScript != null)
            {
                itemScript.Setup(obj);
            }

            TextMeshProUGUI objectNameText = newLine.GetComponentInChildren<TextMeshProUGUI>();
            if (objectNameText != null)
            {
                objectNameText.text = obj.name;
            }
        }
    }

    public void ToggleAllCheckboxes(bool selectAll)
    {
        var selectionHandler = SelectionHandler.GetInstance();
        if (selectionHandler == null) return;

        if (selectAll)
        {
            foreach (var obj in _interactableObjects)
            {
                if (!selectionHandler.IsSelected(obj))
                {
                    selectionHandler.HandleSelection(obj, false);
                }
            }
        }
        else
        {
            selectionHandler.HandleDeselection();
        }
    }

    public void ToggleAllVisibility()
    {
        _allObjectsVisible = !_allObjectsVisible;

        foreach (var obj in _interactableObjects)
        {
            obj.SetActive(_allObjectsVisible);
        }
    }

    public void SetTransparency100()
    {
        SelectionHandler.GetInstance().SetSelectionTransparency(1.0f);
    }

    public void SetTransparency75()
    {
        SelectionHandler.GetInstance().SetSelectionTransparency(0.75f);
    }

    public void SetTransparency50()
    {
        SelectionHandler.GetInstance().SetSelectionTransparency(0.5f);
    }

    public void SetTransparency25()
    {
        SelectionHandler.GetInstance().SetSelectionTransparency(0.25f);
    }
}
