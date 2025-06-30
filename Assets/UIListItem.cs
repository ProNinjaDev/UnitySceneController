using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    public void HandleSelectionToggle()
    {

    }

    public void HandleVisibilityButtonClick()
    {
        
    }
}
