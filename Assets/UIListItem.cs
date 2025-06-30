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
    }

    public void HandleSelectionToggle(bool isSelected)
    {
        if (_referencedObject == null) return;

        SelectionHandler.GetInstance().HandleSelection(_referencedObject);
    }

    public void HandleVisibilityButtonClick()
    {
        bool newVisibility = !_referencedObject.activeSelf;
        _referencedObject.SetActive(newVisibility);
    }
}
