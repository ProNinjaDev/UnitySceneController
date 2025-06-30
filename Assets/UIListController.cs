using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIListController : MonoBehaviour
{
    [SerializeField]
    private GameObject lineOfListPrefab;

    [SerializeField]
    private Transform listContainer;

    void Start()
    {
        GameObject[] interactableObjects = GameObject.FindGameObjectsWithTag("Interactable");

        foreach (var obj in interactableObjects)
        {
            GameObject newLine = Instantiate(lineOfListPrefab);

            newLine.transform.SetParent(listContainer, false);

            TextMeshProUGUI objectNameText = newLine.GetComponentInChildren<TextMeshProUGUI>();

            if (objectNameText != null)
            {
                objectNameText.text = obj.name;
            }
        }
    }

    void Update()
    {
        
    }
}
