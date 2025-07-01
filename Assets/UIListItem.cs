using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIListItem : MonoBehaviour
{
    private GameObject _referencedObject;

    [SerializeField]
    private Toggle selectionToggle;

    [SerializeField]
    private Button button;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Setup(GameObject originalObject)
    {
        _referencedObject = originalObject;

        selectionToggle.onValueChanged.AddListener(HandleSelectionToggle);
        button.onClick.AddListener(HandleVisibilityButtonClick);

        SelectionHandler.GetInstance().SelectionUpdated += UpdateVisuals;
        UpdateVisuals();
    }

    private void OnDestroy()
    {
        if (SelectionHandler.GetInstance() != null)
        {
            SelectionHandler.GetInstance().SelectionUpdated -= UpdateVisuals;
        }
    }

    public void HandleSelectionToggle(bool isSelected)
    {
        if (_referencedObject == null) return;
        
        var handler = SelectionHandler.GetInstance();
        bool isCurrentlySelected = handler.IsSelected(_referencedObject);

        if (isSelected)
        {
            handler.HandleSelection(_referencedObject);
        }
        else
        {
            handler.HandleDeselection(_referencedObject);
        }
    }

    public void HandleVisibilityButtonClick()
    {
        if (_referencedObject != null)
        {
            _referencedObject.SetActive(!_referencedObject.activeSelf);
        }
    }
    
    private void UpdateVisuals()
    {
        if (_referencedObject == null) return;
        
        bool isSelected = SelectionHandler.GetInstance().IsSelected(_referencedObject);
        selectionToggle.SetIsOnWithoutNotify(isSelected);
    }
}
